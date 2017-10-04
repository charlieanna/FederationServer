///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

namespace FederationServer
{
    public class TestHarness : CommunicatorBase
    {
        public string TestStorage { get; set; } = "../../../TestHarness/TestStorage";
        private string RepoStorage { get; } = "../../../Repository/RepoStorage";
        public List<string> Files { get; set; } = new List<string>();

        public override void Execute()
        {
            var testRequest = ParseTestRequest();
            LoadLibraries(testRequest);
            SendLogs();
        }

        private void SendLogs()
        {
            var tempFiles = Directory.GetFiles(".", "test.log");
            for (var i = 0; i < tempFiles.Length; ++i)
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            Files.Clear();
            Files.AddRange(tempFiles);
            MoveFiles();
        }

        private void MoveFiles()
        {
            foreach (var file in Files)
                try
                {
                    var fileName = Path.GetFileName(file);
                    var destSpec = Path.Combine(RepoStorage, fileName ?? throw new InvalidOperationException());
                    File.Copy(file, destSpec, true);
                }
                catch (Exception ex)
                {
                    Console.Write("\n--{0}--", ex.Message);
                }
        }

        private TestRequest ParseTestRequest()
        {
            //read from xml file. 
            var trXml = File.ReadAllText(TestStorage + "/TestRequest.xml");
            var testRequest = trXml.FromXml<TestRequest>();
            var typeName = testRequest.GetType().Name;
            Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
            Console.Write(testRequest);
            Console.WriteLine();
            return testRequest;
        }

        private void LoadLibraries(TestRequest testRequest)
        {
            Console.Write("\n  Demonstrating Robust Test Loader");
            Console.Write("\n ==================================\n");
            foreach (var test in testRequest.tests)
                if (test.toolchain == "csharp")
                {
                    LoadDll(test);
                }
                else if (test.toolchain == "java")
                {
                    LoadJar(test);
                }
        }

        private static void LoadJar(TestElement test)
        {
            var result = JavaLoaderExec.Test(test);
            Console.Write("\n\n  {0}", result);
            Console.Write("\n\n");
        }

        private void LoadDll(TestElement test)
        {
            var loader = new DllLoaderExec(test);
            DllLoaderExec.testersLocation = TestStorage;
            // convert testers relative path to absolute path
            DllLoaderExec.testersLocation = Path.GetFullPath(DllLoaderExec.testersLocation);
            Console.Write("\n  Loading Test Modules from:\n    {0}\n", DllLoaderExec.testersLocation);
            // run load and tests
            var result = loader.loadAndExerciseTesters();
            Console.Write("\n\n  {0}", result);
            Console.Write("\n\n");
        }
    }

    internal class TestTestHarness
    {
#if (TEST_TestHarness)
    static void Main(string[] args)
    {
      Console.Write("\n  Testing Repository Class");
      Console.Write("\n =======================\n");

      Client client = new Client();
      client.Execute();

      Repository repo = new Repository();
      repo.execute();

      Builder builder = new Builder();
      builder.Execute();

      TestHarness harness = new TestHarness();
      harness.Execute();
      Console.WriteLine();
    }
#endif
    }
}