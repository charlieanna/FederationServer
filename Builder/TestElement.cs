///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FederationServer.Build
{
    public class TestElement /* information about a single test */
    {
        public TestElement()
        {
        }

        public TestElement(string name)
        {
            testName = name;
        }

        public TestElement(string testName, string toolchain)
        {
            this.testName = testName;
            this.toolchain = toolchain;
        }

        public string testName { get; set; }
        public string toolchain { get; set; }
        public string testDriver { get; set; }
        public List<string> testCodes { get; set; } = new List<string>();

        public void addDriver(string name)
        {
            testDriver = name;
        }

        public void addCode(string name)
        {
            testCodes.Add(name);
        }

        public override string ToString()
        {
            var temp = "\n    test: " + testName;
            temp += "\n      testDriver: " + testDriver;
            foreach (var testCode in testCodes)
                temp += "\n      testCode:   " + testCode;
            return temp;
        }
    }
}