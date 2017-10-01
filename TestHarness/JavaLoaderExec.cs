using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    public class JavaLoaderExec
    {
        private static string GetJavaInstallationPath()
        {
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
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
                    Console.Write(key.GetValueNames());
                    return key.GetValue("JavaHome").ToString();
                }
            }
        }
        public void test(TestElement testElement)
        {
            string installPath = GetJavaInstallationPath();
            string javaPath = Path.Combine(installPath, "bin\\Java.exe");
            string jarPath = Path.Combine(installPath, "bin\\Jar.exe");
            string javacPath = Path.Combine(installPath, "bin\\Javac.exe");
            string driver = testElement.testDriver.Remove(testElement.testDriver.LastIndexOf("."));
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = javaPath;
                process.StartInfo.Arguments = "-jar " + driver + ".jar"; ///out:" + BuildStorage+"/"+testElement.testDriver + ".dll "
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

    }
}

