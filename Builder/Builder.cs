using FederationServer.Build;
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
        private string RepoStorage { get; set; } = "../../../Repository/RepoStorage";
        private string BuildStorage { get; set; } = "../../../Builder/BuilderStorage";
        private string TestStorage { get; set; } = "../../../TestHarness/TestStorage";
        private List<string> files { get; set; } = new List<string>();
        private List<string> files1 { get; set; } = new List<string>();

        public override void processMessage(Message msg)
        {
            execute();
            msg.to = "TestHarness";
            msg.from = "Builder";
            environ.testHarness.postMessage(msg);
        }
        private void execute()
        {
            BuildRequest buildRequest = ParseBuildRequest();
            BuildLibraries(buildRequest);
            SendLogs();
            CreateTestRequest();
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

        private void BuildLibraries(BuildRequest request)
        {
            foreach (TestElement testElement in request.tests)
            {
                LibraryBuilder lBuilder = new LibraryBuilder();
                lBuilder.build(BuildStorage, testElement);
            }

        }
        private void CreateTestRequest()
        {
            string xml = File.ReadAllText(BuildStorage + "/BuildRequest.xml");
            BuildRequest buildRequest = xml.FromXml<BuildRequest>();


            "Creating Test Request".title('=');
            Console.WriteLine();
            TestRequest tr = new TestRequest();
            tr.author = "Jim Fawcett";
            foreach (Build.TestElement test in buildRequest.tests)
            {
                if (test.toolchain == "csharp")
                {
                    TestElement te1 = new TestElement();
                    te1.testName = "test1";
                    te1.toolchain = "csharp";
                    te1.addDriver(test.testDriver.Remove(test.testDriver.LastIndexOf(".")) + ".dll");
                    te1.addCode(test.testName + ".dll");
                    tr.tests.Add(te1);
                }
                else if (test.toolchain == "java") {
                    TestElement te2 = new TestElement();
                    te2.testName = "test2";
                    te2.toolchain = "java";
                    te2.addDriver(test.testDriver.Remove(test.testDriver.LastIndexOf(".")) + ".java");
                    tr.tests.Add(te2);
                }
            }
            
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(TestStorage + "/TestRequest.xml", trXml);

        }
    }
}
