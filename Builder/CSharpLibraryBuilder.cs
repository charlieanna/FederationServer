///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using FederationServer.Build;

namespace FederationServer
{
    internal class CSharpLibraryBuilder
    {
        public CSharpLibraryBuilder(string buildStorage, TestElement testElement)
        {
            BuildStorage = buildStorage;
            this.testElement = testElement;
        }

        public string BuildStorage { get; }
        public TestElement testElement { get; }

        public void Build()
        {
            var tests = testElement.testCodes;
            var testNames = new List<string>();

            foreach (var test in tests)
                testNames.Add("..\\..\\..\\Builder\\BuilderStorage\\" + test);
            TestDriverBuilder(testElement, testNames);
            testNames.Add("..\\..\\..\\Builder\\BuilderStorage\\" + testElement.testDriver);
            SourceCodeBuilder(testElement, testNames);
        }


        private void SourceCodeBuilder(TestElement testElement, List<string> testNames)
        {
            var frameworkPath = RuntimeEnvironment.GetRuntimeDirectory();
            try
            {
                using (var process = new Process())
                {
                    var driver = testElement.testDriver.Remove(testElement.testDriver.LastIndexOf(".", StringComparison.Ordinal));
                    process.StartInfo.FileName = frameworkPath + "/csc.exe";
                    process.StartInfo.Arguments = "/target:library /out:..\\..\\..\\TestHarness\\TestStorage\\" +
                                                  driver + ".dll " + string.Join(" ", testNames);
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine("Building: " + testElement.testName);
                    Console.WriteLine("Result:" + output);
                    if (output.Contains("error"))
                    {
                    }
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

        private static void TestDriverBuilder(TestElement testElement, List<string> testNames)
        {
            var frameworkPath = RuntimeEnvironment.GetRuntimeDirectory();
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = frameworkPath + "/csc.exe";
                    process.StartInfo.Arguments = "/target:library /out:..\\..\\..\\TestHarness\\TestStorage\\" +
                                                  testElement.testName + ".dll " + string.Join(" ", testNames);
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine("Building Library for Source Code: " + testElement.testName);
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

    internal class TestMessages
    {
#if (TEST_MESSAGES)
    static void Main(string[] args)
    {
      Console.Write("\n  Testing Message Class");
      Console.Write("\n =======================\n");

      Message msg = new Message();
      msg.to = "TH";
      msg.from = "CL";
      msg.type = "basic";
      msg.author = "Fawcett";
      msg.body = "    a body";

      Console.Write("\n  base message:\n    {0}", msg.ToString());
      Console.WriteLine();

      msg.show();
      Console.WriteLine();

      Console.Write("\n  Testing Message.fromString(string)");
      Console.Write("\n ------------------------------------");
      Message parsed = msg.fromString(msg.ToString());
      parsed.show();
      Console.WriteLine();
    }
#endif
    }
}