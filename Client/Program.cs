
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Client
{
    public static class ToAndFromXml
    {
        //----< serialize object to XML >--------------------------------

        static public string ToXml(this object obj)
        {
            // suppress namespace attribute in opening tag

            XmlSerializerNamespaces nmsp = new XmlSerializerNamespaces();
            nmsp.Add("", "");

            var sb = new StringBuilder();
            try
            {
                var serializer = new XmlSerializer(obj.GetType());
                using (StringWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, obj, nmsp);
                }
            }
            catch (Exception ex)
            {
                Console.Write("\n  exception thrown:");
                Console.Write("\n  {0}", ex.Message);
            }
            return sb.ToString();
        }
        //----< deserialize XML to object >------------------------------

        static public T FromXml<T>(this string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(new StringReader(xml));
            }
            catch (Exception ex)
            {
                Console.Write("\n  deserialization failed\n  {0}", ex.Message);
                return default(T);
            }
        }
    }
    public static class Utilities
    {
        public static void title(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }
    public class Program
    {
        public static string RepoStorage { get; set; } = "../../RepoStorage";
        public static string BuildStorage { get; set; } = "../../BuilderStorage";
        public static string TestStorage { get; set; } = "../../TestStorage";
        public static List<string> files { get; set; } = new List<string>();

        static void Main(string[] args)
        {
            CleanDirectories();
            CreateBuildRequest();
            CopyFilesFromRepoStorageToBuildStorage();
            BuildRequest buildRequest = ParseBuildRequest();
            ReadFilesFromBuildStorageAndBuildDLLs(buildRequest);
            SendLogs();
            CreateTestRequest();
            TestRequest testRequest = ParseTestRequest();
            LoadDLL(testRequest);

            // put the test.logger inside the repo storage. 
        }

        private static void CleanDirectories()
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

        private static void SendLogs()
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

        private static TestRequest ParseTestRequest()
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

        private static void CreateBuildRequest()
        {
            "Testing THMessage Class".title('=');
            Console.WriteLine();

            ///////////////////////////////////////////////////////////////
            // Serialize and Deserialize TestRequest data structure

            "Testing Serialization of TestRequest data structure".title();

            TestElement te1 = new TestElement();
            te1.testName = "test1";
            te1.addDriver("TestDriver.cs");
            te1.addCode("Tested1.cs");
            te1.addCode("Tested2.cs");

            //  TestElement te2 = new TestElement();
            //   te2.testName = "test2";
            //   te2.addDriver("td2.cs");
            //   te2.addCode("tc3.cs");
            //   te2.addCode("tc4.cs");

            BuildRequest tr = new BuildRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            //tr.tests.Add(te2);
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(BuildStorage + "/BuildRequest.xml", trXml);

        }

        private static void ReadFilesFromBuildStorageAndBuildDLLs(BuildRequest request)
        {
            // for each test, build a dll for the test driver and a dll for each test codes files. 
            var frameworkPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

            foreach (TestElement testElement in request.tests)
            {
                string driverPath = BuildStorage + "/" + testElement.testDriver;
                List<string> tests = testElement.testCodes;
                List<string> testNames = new List<string>();

                foreach (string test in tests)
                {
                    testNames.Add(test);
                }


                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = frameworkPath + "/csc.exe";
                    process.StartInfo.Arguments = "/target:library /out:" + testElement.testName + ".dll " + String.Join(" ", testNames); ///out:"+BuildStorage+"/"+testElement.testDriver + ".dll "
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"build1.log"))
                    {
                        file.Write(output);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                testNames.Add(testElement.testDriver);
                try
                {
                    Process process = new Process();
                    string driver = testElement.testDriver.Remove(testElement.testDriver.LastIndexOf("."));
                    process.StartInfo.FileName = frameworkPath + "/csc.exe";
                    process.StartInfo.Arguments = "/target:library /out:" + driver + ".dll " + String.Join(" ", testNames); ///out:"+BuildStorage+"/"+testElement.testDriver + ".dll "
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"build.log"))
                    {
                        file.Write(output);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            string[] tempFiles = Directory.GetFiles(".", "*.dll");
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
                    if (!Directory.Exists(TestStorage))
                        Directory.CreateDirectory(TestStorage);
                    string fileName = Path.GetFileName(file);
                    string destSpec = Path.Combine(TestStorage, fileName);
                    File.Copy(file, destSpec, true);
                }
                catch (Exception ex)
                {
                    Console.Write("\n--{0}--", ex.Message);
                }
            }
        }

        private static void LoadDLL(TestRequest testRequest)
        {
            Console.Write("\n  Demonstrating Robust Test Loader");
            Console.Write("\n ==================================\n");

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

        private static BuildRequest ParseBuildRequest()
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

        private static void CopyFilesFromRepoStorageToBuildStorage()
        {
            //read files from the repoStorage and move move to the buildStorage
            string[] tempFiles = Directory.GetFiles(RepoStorage, "*.*");
            for (int i = 0; i < tempFiles.Length; ++i)
            {
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            }
            files.AddRange(tempFiles);
            foreach (string file in files)
            {
                Console.Write("\n  \"{0}\"", file);
                string fileName = Path.GetFileName(file);
                Console.Write("\n  sending \"{0}\" to \"{1}\"", fileName, BuildStorage);
                sendFile(file);
            }

        }

        public static bool sendFile(string fileSpec)
        {
            try
            {
                string fileName = Path.GetFileName(fileSpec);
                string destSpec = Path.Combine(BuildStorage, fileName);
                File.Copy(fileSpec, destSpec, true);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--", ex.Message);
                return false;
            }
        }

        public static void CreateTestRequest()
        {

            "Testing THMessage Class".title('=');
            Console.WriteLine();

            ///////////////////////////////////////////////////////////////
            // Serialize and Deserialize TestRequest data structure

            "Testing Serialization of TestRequest data structure".title();

            TestElement te1 = new TestElement();
            te1.testName = "test1";
            te1.addDriver("td1.dll");
            te1.addCode("tc1.dll");
            te1.addCode("tc2.dll");

            TestElement te2 = new TestElement();
            te2.testName = "test2";
            te2.addDriver("td2.dll");
            te2.addCode("tc3.dll");
            te2.addCode("tc4.dll");

            TestRequest tr = new TestRequest();
            tr.author = "Jim Fawcett";
            tr.tests.Add(te1);
            tr.tests.Add(te2);
            string trXml = tr.ToXml();
            Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);
            File.WriteAllText(TestStorage + "/TestRequest.xml", trXml);

        }
    }
}
