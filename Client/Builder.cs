using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Builder
    {
        public Builder()
        {
            BuildRequest buildRequest = ParseBuildRequest();
            ReadFilesFromBuildStorageAndBuildDLLs(buildRequest);
            SendLogs();
            CreateTestRequest();
        }
        public static string RepoStorage { get; set; } = "../../RepoStorage";
        public static string BuildStorage { get; set; } = "../../BuilderStorage";
        public static string TestStorage { get; set; } = "../../TestStorage";
        public static List<string> files { get; set; } = new List<string>();
        public BuildRequest ParseBuildRequest()
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

        public void SendLogs()
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

        public void ReadFilesFromBuildStorageAndBuildDLLs(BuildRequest request)
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





        public void CreateTestRequest()
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
