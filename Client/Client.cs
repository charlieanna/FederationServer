///////////////////////////////////////////////////////////////////////
// Client.cs - Process that builds the BuildRequest                    //
//                                                                     //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017   //
///////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using FederationServer.Build;

namespace FederationServer
{
    public class Client : CommunicatorBase
    {
     
        private string RepoStorage { get; } = "../../../Repository/RepoStorage";

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
            var te1 = new TestElement("test1", "csharp");
            te1.addDriver("TestDriver.cs");
            te1.addCode("Tested1.cs");
            te1.addCode("Tested2.cs");

            // Java Build pass and Tests pass
            var te2 = new TestElement("test2", "java");
            te2.addDriver("TestDriver.java");
            te2.addCode("Tested1.java");
            te2.addCode("Tested2.java");

            // CSharp Build pass and Tests Fail
            var te3 = new TestElement("test3", "csharp");
            te3.addDriver("TestDriver1.cs");
            te3.addCode("Tested3.cs");

            // CSharp Build Fail
            var te4 = new TestElement("test4", "csharp");
            te4.addDriver("TestDriverBuildFail.cs");
            te4.addCode("Tested1.cs");
            te4.addCode("Tested2.cs");

            var tr = new BuildRequest("Jim Fawcett");
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