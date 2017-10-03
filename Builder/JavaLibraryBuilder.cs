using FederationServer.Build;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    class JavaLibraryBuilder
    {
        public JavaLibraryBuilder(string buildStorage, TestElement testElement)
        {
            BuildStorage = buildStorage;
            this.testElement = testElement;
        }

        public string BuildStorage { get; private set; }
        public TestElement testElement { get; private set; }

        public void build()
        {
            
          
            string driverPath = BuildStorage + "/" + testElement.testDriver;
            List<string> tests = testElement.testCodes;
            List<string> testNames = new List<string>();

            foreach (string test in tests)
            {
                testNames.Add("..\\..\\..\\Builder\\BuilderStorage\\" + test);
            }
            // goes into executive.
            testNames.Add("..\\..\\..\\Builder\\BuilderStorage\\" + testElement.testDriver);
            sourceCodeBuilder(testElement, testNames);
            testDriverBuilder(testElement, testNames);
        }

        private string GetJavaInstallationPath()
        {
            string environmentPath = System.Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return environmentPath;
            }

            string javaKey = "SOFTWARE\\JavaSoft\\Java Development Kit\\";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
            {
                string currentVersion = rk.GetValue("CurrentVersion").ToString();
                using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                {
                    return key.GetValue("JavaHome").ToString();
                }
            }
        }

        private void testDriverBuilder(TestElement testElement, List<string> testNames)
        {
            string installPath = GetJavaInstallationPath();
            string javaPath = Path.Combine(installPath, "bin\\Java.exe");
            string jarPath = Path.Combine(installPath, "bin\\Jar.exe");
            string javacPath = Path.Combine(installPath, "bin\\Javac.exe");
            List<string> tests = testElement.testCodes;
            List<string> classNames = new List<string>();

            foreach (string test in tests)
            {
                string kclass = "..\\..\\..\\Builder\\BuilderStorage\\" + test.Remove(test.LastIndexOf("."));
                classNames.Add(kclass + ".class");
            }
            try
            {
                string driver = testElement.testDriver.Remove(testElement.testDriver.LastIndexOf("."));
                Process process = new Process();
                process.StartInfo.FileName = jarPath;
                process.StartInfo.Arguments = "cfe ..\\..\\..\\TestHarness\\TestStorage\\" + driver +".jar TestDriver TestDriver.class " + String.Join(" ", classNames); ///out:"+BuildStorage+"/"+testElement.testDriver + ".dll "
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"build2.log"))
                {
                    file.Write(output);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private void sourceCodeBuilder(TestElement testElement, List<string> testNames)
        {
            string installPath = GetJavaInstallationPath();
            string javaPath = Path.Combine(installPath, "bin\\Java.exe");
            string jarPath = Path.Combine(installPath, "bin\\Jar.exe");
            string javacPath = Path.Combine(installPath, "bin\\Javac.exe");
            var frameworkPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            try
            {
                //puts the .class files in the build storage
                Process process = new Process();
                process.StartInfo.FileName = javacPath;
                process.StartInfo.Arguments = String.Join(" ", testNames); 
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"build3.log"))
                {
                    file.Write(output);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}

