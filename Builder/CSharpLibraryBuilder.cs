﻿using FederationServer.Build;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FederationServer
{
    class CSharpLibraryBuilder
    {
        
        public string BuildStorage { get; private set; }
        public TestElement testElement { get; private set; }
        public CSharpLibraryBuilder(string buildStorage, TestElement testElement)
        {
            BuildStorage = buildStorage;
            this.testElement = testElement;
        }
        public void build()
        {
            string driverPath = BuildStorage + "/" + testElement.testDriver;

            

            List<string> tests = testElement.testCodes;
            List<string> testNames = new List<string>();

            foreach (string test in tests)
            {
                
                testNames.Add("..\\..\\..\\Builder\\BuilderStorage\\"+ test);
            }
            testDriverBuilder(testElement, testNames);
            testNames.Add("..\\..\\..\\Builder\\BuilderStorage\\"+testElement.testDriver);
            sourceCodeBuilder(testElement, testNames);
        }


        private void sourceCodeBuilder(TestElement testElement, List<string> testNames)
        {
            var frameworkPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            try
            {
                Process process = new Process();
                string driver = testElement.testDriver.Remove(testElement.testDriver.LastIndexOf("."));
                process.StartInfo.FileName = frameworkPath + "/csc.exe";
                process.StartInfo.Arguments = "/target:library /out:..\\..\\..\\TestHarness\\TestStorage\\" + driver + ".dll " + String.Join(" ", testNames); ///out:"+BuildStorage+"/"+testElement.testDriver + ".dll "
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Building: " + testElement.testName);
                Console.WriteLine("Result:" + output);
                if (output.Contains("error"))
                {

                }
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"build.log"))
                {
                    file.WriteLine(output);
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
                process.StartInfo.Arguments = "/target:library /out:..\\..\\..\\TestHarness\\TestStorage\\" + testElement.testName + ".dll " + String.Join(" ", testNames); ///out:"+BuildStorage+"/"+testElement.testDriver + ".dll "
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Building Library for Source Code: " + testElement.testName);
                Console.WriteLine("Result:" + output);
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
