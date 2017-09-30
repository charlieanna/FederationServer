using System;
using System.Collections.Generic;

namespace Client
{
    public class BuildRequest  /* a container for one or more TestElements */
    {
        public string author { get; set; }
        public List<TestElement> tests { get; set; } = new List<TestElement>();

        public BuildRequest() { }
        public BuildRequest(string auth)
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