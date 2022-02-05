using Server.Tcp;
using System.Net;
using Server.Server.Handler;
using System.Threading;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            IClientHandler codeClientHandler = new CodeClientHandler();
            TcpServer server = new TcpServer(8000, IPAddress.Parse("127.0.0.1"), codeClientHandler.HandleClient);
            Thread codeServer = new Thread(server.Listen);
            codeServer.Start();

            IClientHandler messageClientHandler = new MessageClientHandler();
            TcpServer messageServer = new TcpServer(8001, IPAddress.Parse("127.0.0.1"), messageClientHandler.HandleClient);
            messageServer.Listen();
        }
    }
}
