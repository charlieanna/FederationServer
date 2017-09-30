using FederationServer.Build;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    class LibraryBuilder
    {
        public void build(string BuildStorage, TestElement testElement)
        {
            string driverPath = BuildStorage + "/" + testElement.testDriver;
            List<string> tests = testElement.testCodes;
            List<string> testNames = new List<string>();

            foreach (string test in tests)
            {
                testNames.Add(test);
            }
            string frameworkPath = testDriverBuilder(testElement, testNames);
            testNames.Add(testElement.testDriver);
            sourceCodeBuilder (testElement, testNames, frameworkPath);
        }

        private static void sourceCodeBuilder(TestElement testElement, List<string> testNames, string frameworkPath)
        {
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

        private static string testDriverBuilder(TestElement testElement, List<string> testNames)
        {
            var frameworkPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
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

            return frameworkPath;
        }
    }
}
