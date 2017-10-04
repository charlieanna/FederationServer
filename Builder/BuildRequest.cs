///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FederationServer.Build;

namespace FederationServer
{
    public class BuildRequest /* a container for one or more TestElements */
    {
        public BuildRequest()
        {
        }

        public BuildRequest(string auth)
        {
            author = auth;
            tests = new List<TestElement>();
        }

        public string author;
        public List<TestElement> tests;

        public override string ToString()
        {
            var temp = "\n  author: " + author;
            foreach (var te in tests)
                temp += te.ToString();
            return temp;
        }
    }
}