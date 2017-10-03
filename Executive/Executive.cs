
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FederationServer
{
    public class Executive : CommunicatorBase
    {
        public Executive()
        {
            string BuildStorage = "../../../Builder/BuilderStorage";
            string TestStorage = "../../../TestHarness/TestStorage";
            //if (Directory.Exists(BuildStorage))
            //    Directory.Delete(BuildStorage, true);
            //if (Directory.Exists(TestStorage))
            //    Directory.Delete(TestStorage, true);
            environ.client = new Client();
            environ.repo = new Repository();
            environ.builder = new Builder();
            environ.testHarness = new TestHarness();

        }
        public void doop()
        {
            Message msg = Message.makeMsg("test", "clnt", "exec", "this is a message flow test");
            environ.client.postMessage(msg);
        }
    }

    public class TestMsgPass
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Starting Federation Server");
            Console.Write("\n ================================================");

            Executive exec = new Executive();   // builds federation components
            exec.doop();                        // starts federation processing
            Environment.wait();
            Console.Write("\n\n");
        }
    }
}
