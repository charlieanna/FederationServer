/////////////////////////////////////////////////////////////////////
// CommunicatorBase.cs - base for all parts of the Federation      //
//                                                                 //
// Jim Fawcett, Ankur Kothari CSE681 - Software Modeling and Analysis, Fall 2017 //
/////////////////////////////////////////////////////////////////////
namespace FederationServer
{
    public abstract class CommunicatorBase : ICommunicator
    {
        protected static Environment environ;
        
        public virtual void Execute()
        {
        }
    }
}