using System.Collections.Generic;
using System.Threading;

namespace FederationServer
{
    public interface ICommunicator
    {
        void Execute();
    }

    public struct Environment
    {
        public ICommunicator client { get; set; }
        public ICommunicator repo { get; set; }
        public ICommunicator builder { get; set; }
        public ICommunicator testHarness { get; set; }
        public static List<Thread> threadList { get; set; } = new List<Thread>();

        public static void wait()
        {
            foreach (var t in threadList)
                t.Join();
        }
    }

    public class Message
    {
        public string type { get; set; } = "BuildRequest";
        public string to { get; set; } = "";
        public string from { get; set; } = "";
        public string body { get; set; } = "";

        public static Message makeMsg(string type, string to, string from, string body)
        {
            var msg = new Message
            {
                type = type,
                to = to,
                from = from,
                body = body
            };
            return msg;
        }

        public override string ToString()
        {
            var outStr = "Message - " +
                         string.Format("type: {0}, ", type) +
                         string.Format("from: {0}, ", from) +
                         string.Format("to: {0}, ", to) +
                         string.Format("body: {0}, ", body);
            return outStr;
        }
    }
}