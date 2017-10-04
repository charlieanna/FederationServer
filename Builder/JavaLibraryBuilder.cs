///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FederationServer.Build;
using Microsoft.Win32;

namespace FederationServer
{
    internal class JavaLibraryBuilder
    {
        public JavaLibraryBuilder(string buildStorage, TestElement testElement)
        {
            BuildStorage = buildStorage;
            this.TestElement = testElement;
        }

        public string BuildStorage { get; }
        public TestElement TestElement { get; }

        public void Build()
        {
            var tests = TestElement.testCodes;
            var testNames = new List<string>();

            foreach (var test in tests)
                testNames.Add(test);
            var installPath = GetJavaInstallationPath();
            if (installPath == String.Empty)
            {
                Console.WriteLine("Seems like you don't have Java installed on your system.");
                return;
            }
            testNames.Add(TestElement.testDriver); //"..\\..\\..\\Builder\\BuilderStorage\\" +
            SourceCodeBuilder(installPath, TestElement, testNames);
            TestDriverBuilder(installPath, TestElement, testNames);
        }

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
                    using (var key = rk.OpenSubKey(currentVersion) ?? throw new ArgumentNullException("rk.OpenSubKey(currentVersion)"))
                    {
                        return key.GetValue("JavaHome").ToString();
                    }
                }
                return string.Empty;
            }
        }

        private static void TestDriverBuilder(string installPath, TestElement testElement, List<string> testNames)
        {
            var jarPath = Path.Combine(installPath, "bin\\Jar.exe");
            var tests = testNames;
            var classNames = new List<string>();
            foreach (var test in tests)
            {
                var kclass = test.Remove(test.LastIndexOf("."));
                var fileName = kclass + ".class";
                classNames.Add(fileName);
            }
            try
            {
                var driver = testElement.testDriver.Remove(testElement.testDriver.LastIndexOf("."));
                using (var process = new Process())
                {
                    process.StartInfo.FileName = jarPath;
                    process.StartInfo.WorkingDirectory = "..\\..\\..\\Builder\\BuilderStorage\\";
                    process.StartInfo.Arguments =
                        "cfe " + driver + ".jar " + driver + " " +
                        string.Join(" ", classNames); // ..\\..\\..\\TestHarness\\TestStorage\\
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine("Building Java Library for : " + testElement.testName);
                    Console.WriteLine(output);
                    using (StreamWriter w = File.AppendText(@"build.log"))
                    {
                        w.WriteLine(output);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private static void SourceCodeBuilder(string installPath, TestElement testElement, List<string> testNames)
        {
            var javacPath = Path.Combine(installPath, "bin\\Javac.exe");
            try
            {
                //puts the .class files in the build storage
                using (var process = new Process())
                {
                    process.StartInfo.FileName = javacPath;
                    process.StartInfo.WorkingDirectory = "..\\..\\..\\Builder\\BuilderStorage\\";
                    process.StartInfo.Arguments = string.Join(" ", testNames);
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine("Compiling Java classes for: " + testElement.testName);
                    Console.WriteLine("Result:" + output);
                    using (StreamWriter w = File.AppendText(@"build.log"))
                    {
                        w.WriteLine(output);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}