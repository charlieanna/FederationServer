﻿///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FederationServer
{
    public class TestRequest /* a container for one or more TestElements */
    {
        public TestRequest()
        {
        }

        public TestRequest(string auth)
        {
            author = auth;
            tests  = new List<TestElement>();
        }

        public string author { get; set; }
        public List<TestElement> tests { get; set; } 
    }
}