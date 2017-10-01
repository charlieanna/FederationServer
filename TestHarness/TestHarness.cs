using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{

    public class TestHarness
    {

        public TestHarness()
        {
            TestRequest testRequest = ParseTestRequest();
            LoadDLL(testRequest);
            // put the test.logger inside the repo storage.
        }
        public static string TestStorage { get; set; } = "../../../TestHarness/TestStorage";
        public static List<string> files { get; set; } = new List<string>();
        public TestRequest ParseTestRequest()
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

        public void LoadDLL(TestRequest testRequest)
        {
            Console.Write("\n  Demonstrating Robust Test Loader");
            Console.Write("\n ==================================\n");
            foreach (TestElement test in testRequest.tests)
            {
                if (test.toolchain == "csharp")
                {
                    DllLoaderExec loader = new DllLoaderExec();

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
