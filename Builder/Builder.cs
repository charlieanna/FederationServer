﻿using FederationServer.Build;
using SWTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    public class Builder : CommunicatorBase
    {
        public Builder()
        {
            rcvQ = new BlockingQueue<Message>();
            start();
        }
        public static string RepoStorage { get; set; } = "../../../Repository/RepoStorage";
        public static string BuildStorage { get; set; } = "../../../Builder/BuilderStorage";
        public static string TestStorage { get; set; } = "../../../TestHarness/TestStorage";
        public static List<string> files { get; set; } = new List<string>();
        public static List<string> files1 { get; set; } = new List<string>();

        private void execute()
        {
            BuildRequest buildRequest = ParseBuildRequest();
            BuildDLLs(buildRequest);
            SendLogs();
            CreateTestRequest();
        }

        public override void processMessage(Message msg)
        {
            execute();
            msg.to = "TestHarness";
            msg.from = "Builder";
            environ.testHarness.postMessage(msg);
        }
        private BuildRequest ParseBuildRequest()
        {
            //read from xml file. 
            string trXml = File.ReadAllText(BuildStorage + "/BuildRequest.xml");
            BuildRequest buildRequest = trXml.FromXml<BuildRequest>();
            string typeName = buildRequest.GetType().Name;
            Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
            Console.Write(buildRequest);
            Console.WriteLine();
            return buildRequest;

        }

        private void SendLogs()
        {
            string[] tempFiles = Directory.GetFiles(".", "*.log");
            for (int i = 0; i < tempFiles.Length; ++i)
            {
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            }
            files.Clear();
            files.AddRange(tempFiles);
            foreach (string file in files)
            {
                try
                {
                    string fileName = Path.GetFileName(file);
                    string destSpec = Path.Combine(RepoStorage, fileName);
                    File.Copy(file, destSpec, true);
                }
                catch (Exception ex)
                {
                    Console.Write("\n--{0}--", ex.Message);
                }
            }
        }

        private void BuildDLLs(BuildRequest request)
        {
            foreach (TestElement testElement in request.tests)
            {
                LibraryBuilder lBuilder = new LibraryBuilder();
                lBuilder.build(BuildStorage, testElement);
            }
            string[] tempFiles = Directory.GetFiles(".", "*.dll");
            for (int i = 0; i < tempFiles.Length; ++i)
            {
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            }
            files.Clear();
            files.AddRange(tempFiles);
            foreach (string file in files)
            {
                try
                {
                    if (!Directory.Exists(TestStorage))
                        Directory.CreateDirectory(TestStorage);
                    string fileName = Path.GetFileName(file);
                    string destSpec = Path.Combine(TestStorage, fileName);
                    File.Copy(file, destSpec, true);
                }
                catch (Exception ex)
                {
                    Console.Write("\n--{0}--", ex.Message);
                }
            }

            string[] tempFiles1 = Directory.GetFiles(".", "*.jar");
            for (int i = 0; i < tempFiles1.Length; ++i)
            {
                tempFiles1[i] = Path.GetFullPath(tempFiles1[i]);
            }
            files1.Clear();
            files1.AddRange(tempFiles1);
            foreach (string file in files1)
            {
                try
                {
                    if (!Directory.Exists(TestStorage))
                        Directory.CreateDirectory(TestStorage);
                    string fileName = Path.GetFileName(file);
                    string destSpec = Path.Combine(TestStorage, fileName);
                    File.Copy(file, destSpec, true);
                }
                catch (Exception ex)
                {
                    Console.Write("\n--{0}--", ex.Message);
                }
            }
        }





        private void CreateTestRequest()
        {

            "Testing THMessage Class".title('=');
            Console.WriteLine();

            ///////////////////////////////////////////////////////////////
            // Serialize and Deserialize TestRequest data structure

            "Testing Serialization of TestRequest data structure".title();

            TestElement te1 = new TestElement();
            te1.testName = "test1";
            te1.toolchain = "csharp";
            te1.addDriver("TestDriver.dll");
            te1.addCode("tc1.dll");
            te1.addCode("tc2.dll");

            TestElement te2 = new TestElement();
            te2.testName = "test2";
            te2.toolchain = "java";
            te2.addDriver("TestDriver.jar");
            te2.addCode("tc3.dll");
            te2.addCode("tc4.dll");

            TestRequest tr = new TestRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(TestStorage + "/TestRequest.xml", trXml);

        }
    }
}
