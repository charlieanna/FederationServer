///////////////////////////////////////////////////////////////////////
// Client.cs - Process that builds the BuildRequest                    //
//                                                                     //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017   //
///////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using FederationServer.Build;
using SWTools;
using FederationServer;

namespace FederationServer
{
    public class Client : CommunicatorBase
    {
        public Client()
        {
            rcvQ = new BlockingQueue<Message>();
            start();
        }

        private string RepoStorage { get; } = "../../../Repository/RepoStorage";

        public override void processMessage(Message msg)
        {
            Execute();
            msg.to = "repo";
            msg.from = "clnt";
            environ.repo.postMessage(msg);
        }

        public override void Execute()
        {
            CreateBuildRequest();
            environ.repo.Execute();
        }


        private void CreateBuildRequest()
        {
            ///////////////////////////////////////////////////////////////
            // Serialize Build Request data structure

            "Creating Build Request".Title();
            Console.WriteLine();

            // CSharp Build pass and Tests pass
            var te1 = new TestElement
            {
                toolchain = "csharp",
                testName = "test1"
            };
            te1.addDriver("TestDriver.cs");
            te1.addCode("Tested1.cs");
            te1.addCode("Tested2.cs");

            // Java Build pass and Tests pass
            var te2 = new TestElement
            {
                testName = "test2",
                toolchain = "java"
            };
            te2.addDriver("TestDriver.java");
            te2.addCode("Tested1.java");
            te2.addCode("Tested2.java");

            // CSharp Build pass and Tests Fail
            var te3 = new TestElement
            {
                toolchain = "csharp",
                testName = "test3"
            };
            te3.addDriver("TestDriver1.cs");
            te3.addCode("Tested3.cs");

            // CSharp Build Fail
            var te4 = new TestElement
            {
                toolchain = "csharp",
                testName = "test4"
            };
            te4.addDriver("TestDriverBuildFail.cs");
            te4.addCode("Tested1.cs");
            te4.addCode("Tested2.cs");


            var tr = new BuildRequest
            {
                author = "Jim Fawcett"
            };
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            tr.tests.Add(te3);
            tr.tests.Add(te4);
            var trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(RepoStorage + "/BuildRequest.xml", trXml);
        }
    }

    internal class TestClient
    {
#if (TEST_Client)
    static void Main(string[] args)
    {
      Console.Write("\n  Testing Client Class");
      Console.Write("\n =======================\n");

      var client = new Client();
      client.Execute();

      Console.WriteLine();
    }
#endif
    }
}