///////////////////////////////////////////////////////////////////////
// Environment.cs - Shared Code for Federation Message-Passing Demo  //
//                                                                   //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017   //
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FederationServer
{
  public interface ICommunicator
  {
    void postMessage(Message msg);
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
      foreach (Thread t in Environment.threadList)
      {
        t.Join();
      }
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
      Message msg = new Message();
      msg.type = type;
      msg.to = to;
      msg.from = from;
      msg.body = body;
      return msg;
    }
    public override string ToString()
    {
      string outStr = "Message - " +
        string.Format("type: {0}, ", type) +
        string.Format("from: {0}, ", from) +
        string.Format("to: {0}, ", to) +
        string.Format("body: {0}, ", body);
      return outStr;
    }
  }
}
