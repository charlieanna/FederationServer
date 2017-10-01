using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    public class Client
    {
        public Client()
        {
            CleanDirectories();
            CreateBuildRequest();
        }
        public static string RepoStorage { get; set; } = "../../../Repository/RepoStorage";
       
        public void CleanDirectories()
        {
            if (!Directory.Exists(RepoStorage))
                Directory.CreateDirectory(RepoStorage);
            
            
        }

        public void CreateBuildRequest()
        {
            "Testing THMessage Class".title('=');
            Console.WriteLine();

            ///////////////////////////////////////////////////////////////
            // Serialize and Deserialize TestRequest data structure

            "Testing Serialization of TestRequest data structure".title();

            Build.TestElement te1 = new Build.TestElement();
            te1.toolchain = "csharp";
            te1.testName = "test1";
            te1.addDriver("TestDriver.cs");
            te1.addCode("Tested1.cs");
            te1.addCode("Tested2.cs");

            Build.TestElement te2 = new Build.TestElement();
            te2.testName = "test2";
            te2.toolchain = "java";
            te2.addDriver("TestDriver.java");
            te2.addCode("Tested1.java");
            te2.addCode("Tested2.java");

            BuildRequest tr = new BuildRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(RepoStorage + "/BuildRequest.xml", trXml);

        }
    }
}
