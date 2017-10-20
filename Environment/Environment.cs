

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
    }
}