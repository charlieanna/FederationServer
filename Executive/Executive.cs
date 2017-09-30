///////////////////////////////////////////////////////////////////////
// Executive.cs - Mock Executive for Federation Message-Passing Demo //
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
  public class Executive : CommunicatorBase
  {
    public Executive()
    {
      environ.client = new Client();
      environ.repo = new Repo();
      environ.builder = new Builder();
      environ.testHarness = new TestHarness();
    }
    public void doOp()
    {
      Message msg = Message.makeMsg("test", "clnt", "exec", "this is a message flow test");
      environ.client.postMessage(msg);
    }
  }
  public class TestMsgPass
  {
    static void Main(string[] args)
    {
      Console.Write("\n  Demonstrating Message Flows in Mock Federation");
      Console.Write("\n ================================================");

      Executive exec = new Executive();   // builds federation components
      exec.doOp();                        // starts federation processing
      Environment.wait();                 // waits for all components to shut down
      Console.Write("\n\n");
    }
  }
}
