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
        public static string RepoStorage { get; set; } = "../../RepoStorage";
        public static string BuildStorage { get; set; } = "../../BuilderStorage";
        public static string TestStorage { get; set; } = "../../TestStorage";
        public void CleanDirectories()
        {
            if (!Directory.Exists(RepoStorage))
                Directory.CreateDirectory(RepoStorage);
            if (!Directory.Exists(BuildStorage))
                Directory.CreateDirectory(BuildStorage);
            if (!Directory.Exists(TestStorage))
                Directory.CreateDirectory(TestStorage);
            System.IO.DirectoryInfo di = new DirectoryInfo(BuildStorage);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            DirectoryInfo di1 = new DirectoryInfo(TestStorage);
            foreach (FileInfo file in di1.GetFiles())
            {
                file.Delete();
            }
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
            te2.addCode("Tested.java");

            BuildRequest tr = new BuildRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(BuildStorage + "/BuildRequest.xml", trXml);

        }
    }
}
