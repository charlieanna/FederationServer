using System.Collections.Generic;

namespace FederationServer.Build
{
    public class TestElement  /* information about a single test */
    {
        public string testName { get; set; }
        public string toolchain { get; set; }
        public string testDriver { get; set; }
        public List<string> testCodes { get; set; } = new List<string>();

        public TestElement() { }
        public TestElement(string name)
        {
            testName = name;
        }
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
            string temp = "\n    test: " + testName;
            temp += "\n      testDriver: " + testDriver;
            foreach (string testCode in testCodes)
                temp += "\n      testCode:   " + testCode;
            return temp;
        }
    }

}
