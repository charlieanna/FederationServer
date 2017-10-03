using SWTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    public class Client : CommunicatorBase
    {
        private string RepoStorage { get; set; } = "../../../Repository/RepoStorage";
        public Client()
        {
            rcvQ = new BlockingQueue<Message>();
            start();
        }

        public override void processMessage(Message msg)
        {
            execute();
            msg.to = "repo";
            msg.from = "clnt";
            environ.repo.postMessage(msg);
        }

        private void execute()
        {
            CreateBuildRequest();
        }


        private void CreateBuildRequest()
        {
            ///////////////////////////////////////////////////////////////
            // Serialize Build Request data structure

            "Creating Build Request".title();
            Console.WriteLine();

            // CSharp Build pass and Tests pass
            Build.TestElement te1 = new Build.TestElement();
            te1.toolchain = "csharp";
            te1.testName = "test1";
            te1.addDriver("TestDriver.cs");
            te1.addCode("Tested1.cs");
            te1.addCode("Tested2.cs");

            // Java Build pass and Tests pass
            Build.TestElement te2 = new Build.TestElement();
            te2.testName = "test2";
            te2.toolchain = "java";
            te2.addDriver("TestDriver.java");
            te2.addCode("Tested1.java");
            te2.addCode("Tested2.java");

            // CSharp Build pass and Tests Fail
            Build.TestElement te3 = new Build.TestElement();
            te3.toolchain = "csharp";
            te3.testName = "test3";
            te3.addDriver("TestDriver1.cs");
            te3.addCode("Tested3.cs");

            // CSharp Build Fail
            Build.TestElement te4 = new Build.TestElement();
            te4.toolchain = "csharp";
            te4.testName = "test4";
            te4.addDriver("TestDriverBuildFail.cs");
            te4.addCode("Tested1.cs");
            te4.addCode("Tested2.cs");


            BuildRequest tr = new BuildRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            tr.tests.Add(te3);
            tr.tests.Add(te4);
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(RepoStorage + "/BuildRequest.xml", trXml);
        }
    }
}
