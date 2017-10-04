///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;


namespace FederationServer
{
    public class JavaLoaderExec
    {
        private static string GetJavaInstallationPath()
        {
            var environmentPath = System.Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
                return environmentPath;

            var javaKey = "SOFTWARE\\JavaSoft\\Java Development Kit\\";
            using (var rk = Registry.LocalMachine.OpenSubKey(javaKey))
            {
                if (rk != null)
                {
                    var currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (var key = rk.OpenSubKey(currentVersion))
                    {
                        if (key != null) return key.GetValue("JavaHome").ToString();
                    }
                }
                return @"";
            }
        }

        public static string Test(TestElement testElement)
        {
            Console.Write("\n  loaded {0}", testElement.testDriver);
            var installPath = GetJavaInstallationPath();
            var javaPath = Path.Combine(installPath, @"bin\Java.exe");
            var driver =
                testElement.testDriver.Remove(testElement.testDriver.LastIndexOf(@".", StringComparison.Ordinal));
            if (testElement.testDriver.Length > testElement.testDriver.LastIndexOf(".", StringComparison.Ordinal))
                try
                {
                    using (var process = new Process())
                    {
                        LoadJar(javaPath, driver, process);
                        LogOutput(testElement, process);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            return "true";
        }

        private static void LogOutput(TestElement testElement, Process process)
        {
            var output = process.StandardOutput.ReadToEnd();
            var filestream = new FileStream("test.log", FileMode.Append, FileAccess.Write);
            var streamwriter = new StreamWriter(filestream)
            {
                AutoFlush = true
            };
            var currentOut = Console.Out;
            Console.SetOut(streamwriter);
            Console.WriteLine("\n{0}  {1}\n", testElement.testName, output);
            streamwriter.Flush();
            Console.SetOut(currentOut);
            streamwriter.Close();
            filestream.Close();
            Console.WriteLine();
            Console.WriteLine("Done");
        }

        private static void LoadJar(string javaPath, string driver, Process process)
        {
            process.StartInfo.FileName = javaPath;
            process.StartInfo.Arguments = "-jar " + driver + ".jar";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
        }
    }
}