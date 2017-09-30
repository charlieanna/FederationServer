///////////////////////////////////////////////////////////////////////
// TestHarness.cs - Mock Tester for Federation Message-Passing Demo  //
//                                                                   //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017   //
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWTools;

namespace FederationServer
{
  public class TestHarness : CommunicatorBase
  {
    public TestHarness()
    {
      rcvQ = new BlockingQueue<Message>();
      start();
    }
    public override void processMessage(Message msg)
    {
      msg.to = "clnt";
      msg.from = "thrn";
      msg.body = "quit";
      environ.client.postMessage(msg);
    }
  }

}
