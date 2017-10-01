
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FederationServer
{

    public class Program
    {
        static void Main(string[] args)
        {
            string RepoStorage  = "../../../Repository/RepoStorage";
            string BuildStorage = "../../../Builder/BuilderStorage";
            string TestStorage = "../../../TestHarness/TestStorage";
            //Directory.Delete(RepoStorage);
            //Directory.Delete(BuildStorage);
            //Directory.Delete(TestStorage);
            Client client = new Client();
            Repository repository = new Repository();
            Builder builder = new Builder();
            TestHarness testHarness = new TestHarness();
        }
    }
}
