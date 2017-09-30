using System;
using System.Collections.Generic;

namespace Builder
{
    public class TestRequest  /* a container for one or more TestElements */
    {
        public string author { get; set; }
        public List<TestElement> tests { get; set; } = new List<TestElement>();

        public TestRequest() { }
        public TestRequest(string auth)
        {
            author = auth;
        }
        public override string ToString()
        {
            string temp = "\n  author: " + author;
            foreach (TestElement te in tests)
                temp += te.ToString();
            return temp;
        }
    }
}