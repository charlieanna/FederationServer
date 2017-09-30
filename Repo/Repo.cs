/////////////////////////////////////////////////////////////////////
// Repo.cs - Mock Repository for Federation Message-Passing Demo   //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWTools;

namespace FederationServer
{
  public class Repo : CommunicatorBase
  {
    public Repo()
    {
      rcvQ = new BlockingQueue<Message>();
      start();
    }
    public override void processMessage(Message msg)
    {
      if (msg.from == "thrn")
      {
        msg.body = "quit";
      }
      msg.to = "bldr";
      msg.from = "repo";
      environ.builder.postMessage(msg);
    }
  }
}
