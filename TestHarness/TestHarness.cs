using SWTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{

    public class TestHarness : CommunicatorBase
    {

        public TestHarness()
        {
            rcvQ = new BlockingQueue<Message>();
            start();
            // put the test.logger inside the repo storage.
        }

        public override void processMessage(Message msg)
        {
            execute();
            msg.to = "clnt";
            msg.from = "thrn";
            msg.body = "quit";
            environ.client.postMessage(msg);
        }
        public  string TestStorage { get; set; } = "../../../TestHarness/TestStorage";
        public  List<string> files { get; set; } = new List<string>();
        private void execute()
        {
            TestRequest testRequest = ParseTestRequest();
            LoadDLL(testRequest);
        }
        private TestRequest ParseTestRequest()
        {
            //read from xml file. 
            string trXml = File.ReadAllText(TestStorage + "/TestRequest.xml");
            TestRequest testRequest = trXml.FromXml<TestRequest>();
            string typeName = testRequest.GetType().Name;
            Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
            Console.Write(testRequest);
            Console.WriteLine();
            return testRequest;
        }

        private void LoadDLL(TestRequest testRequest)
        {
            Console.Write("\n  Demonstrating Robust Test Loader");
            Console.Write("\n ==================================\n");
            foreach (TestElement test in testRequest.tests)
            {
                if (test.toolchain == "csharp")
                {
                    DllLoaderExec loader = new DllLoaderExec(test);

                    DllLoaderExec.testersLocation = TestStorage;

                    // convert testers relative path to absolute path

                    DllLoaderExec.testersLocation = Path.GetFullPath(DllLoaderExec.testersLocation);
                    Console.Write("\n  Loading Test Modules from:\n    {0}\n", DllLoaderExec.testersLocation);

                    // run load and tests

                    string result = loader.loadAndExerciseTesters();

                    Console.Write("\n\n  {0}", result);
                    Console.Write("\n\n");
                }
                else if(test.toolchain == "java")
                {
                    JavaLoaderExec loader = new JavaLoaderExec();
                    string result = loader.test(test);
                    Console.Write("\n\n  {0}", result);
                    Console.Write("\n\n");
                }
            }
        }

    }
}
