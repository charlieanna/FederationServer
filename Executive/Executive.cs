///////////////////////////////////////////////////////////////////////////
// Executive.cs - Process that starts all the other processes and commands //
// the client to start.                                                    //
// Ankur Kothari, CSE681 - Software Modeling and Analysis, Fall 2017       //
///////////////////////////////////////////////////////////////////////////

using FederationServer;
using System;
using System.IO;
namespace Executive
{
    public class Executive : CommunicatorBase
    {
        public static void Load()
        {
            environ.client = new Client();
            environ.repo = new Repository();
            environ.builder = new Builder();
            environ.testHarness = new TestHarness();
        }

        public static void Start()
        {
            environ.client.Execute();
        }

        public static void CleanDirectories()
        {
            var testStorage = "../../../TestHarness/TestStorage";
            var buildStorage = "../../../Builder/BuilderStorage";
            if (Directory.Exists(testStorage))
            {
                var di = new DirectoryInfo(testStorage);

                foreach (var file in di.GetFiles())
                    file.Delete();
            }
            if (Directory.Exists(buildStorage))
            {
                var di1 = new DirectoryInfo(buildStorage);

                foreach (var file in di1.GetFiles())
                    file.Delete();
            }
        }
    }

    public class TestMsgPass
    {
        private static void Main()
        {
            try
            {
                Console.Write("\n  Starting Federation Server");
                Console.Write("\n ================================================");

                Executive.Load(); // builds federation components
                Executive.CleanDirectories();
                Executive.Start(); // starts federation processing
                Console.Write("\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e.Message);
            }
        }
    }
}