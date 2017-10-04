/////////////////////////////////////////////////////////////////////
// CommunicatorBase.cs - base for all parts of the Federation      //
//                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using SWTools;

namespace FederationServer
{
    public abstract class CommunicatorBase : ICommunicator
    {
        protected static Environment environ;
        protected BlockingQueue<Message> rcvQ = null;
        protected Thread thrd;

        public Thread thread { get; set; }

        public void postMessage(Message msg)
        {
            rcvQ.enQ(msg);
        }

        public virtual void Execute()
        {
        }

        public Thread start()
        {
            thrd = new Thread(
                () =>
                {
                    while (true)
                    {
                        var msg = rcvQ.deQ();
                        processMessage(msg);
                        if (msg.body == "quit")
                        {
                            Console.Write("\n  {0} thread quitting", msg.to);
                            break;
                        }
                    }
                }
            );
            thrd.IsBackground = true;
            thrd.Start();
            Environment.threadList.Add(thrd);
            thread = thrd;
            return thrd;
        }

        public virtual void processMessage(Message msg)
        {
        }

        public void wait()
        {
            thread.Join(); // only waits for own thread
        }
    }
}