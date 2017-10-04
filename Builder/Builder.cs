///////////////////////////////////////////////////////////////////////////
// Builder.cs - Process that Parses the Build Request, Builds the Libraries //
// and commands the TestHarness to start executing the drivers.             //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017        //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using FederationServer.Build;

namespace FederationServer
{
    public class Builder : CommunicatorBase
    {
        private string RepoStorage { get; } = "../../../Repository/RepoStorage";
        private string BuildStorage { get; } = "../../../Builder/BuilderStorage";
        private string TestStorage { get; } = "../../../TestHarness/TestStorage";
        private List<string> Files { get; } = new List<string>();

        public override void Execute()
        {
            var buildRequest = ParseBuildRequest();
            BuildLibraries(buildRequest);
            SendLogs();
            CreateTestRequest();
            environ.testHarness.Execute(); //commands TestHarness to start testing. 
        }

        private BuildRequest ParseBuildRequest()
        {
            //read from xml file. 
            var trXml = File.ReadAllText(BuildStorage + "/BuildRequest.xml");
            var buildRequest = trXml.FromXml<BuildRequest>();
            var typeName = buildRequest.GetType().Name;
            Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
            Console.Write(buildRequest);
            Console.WriteLine();
            return buildRequest;
        }

        private void SendLogs()
        {
            var tempFiles = Directory.GetFiles(".", "build.log");
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

        private void BuildLibraries(BuildRequest request)
        {
            foreach (var testElement in request.tests)
            {
                LibraryBuilder.Build(BuildStorage, testElement);
            }
        }

        // Creates TestRequest.xml and puts it in the TestStorage
        private void CreateTestRequest()
        {
            var xml = File.ReadAllText(BuildStorage + "/BuildRequest.xml");
            var buildRequest = xml.FromXml<BuildRequest>();
            "Creating Test Request".Title('=');
            Console.WriteLine();
            var tr = BuildTestRequest(buildRequest);
            var trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(TestStorage + "/TestRequest.xml", trXml);
        }

        //Builds the Testrequest instance from the build request. 
        private static TestRequest BuildTestRequest(BuildRequest buildRequest)
        {
            var tr = new TestRequest("Jim Fawcett");
            foreach (var test in buildRequest.tests)
            {
                var te = new TestElement(test.testName, test.toolchain);
                if (test.toolchain == "csharp")
                {
                    te.addDriver(test.testDriver.Remove(test.testDriver.LastIndexOf(".", StringComparison.Ordinal)) + ".dll");
                    te.addCode(test.testName + ".dll");
                }
                else if (test.toolchain == "java")
                {
                    te.addDriver(test.testDriver.Remove(test.testDriver.LastIndexOf(".", StringComparison.Ordinal)) + ".java");
                }
                tr.tests.Add(te);
            }
            return tr;
        }
    }

    internal class TestBuilder
    {
#if (TEST_Builder)
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
      Console.WriteLine();
    }
#endif
    }
}