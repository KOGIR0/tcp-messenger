using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server.Tcp
{
    class TcpServer
    {
        int port = 8000; // порт для прослушивания подключений
        IPAddress ipAddress;
        TcpListener server = null;
        WaitCallback clientHandler;

        public TcpServer(int port, IPAddress addr, WaitCallback clientHandler)
        {
            this.port = port;
            this.ipAddress = addr;
            server = new TcpListener(this.ipAddress, this.port);
            this.clientHandler = clientHandler;
        }

        public void Listen()
        {
            try
            {
                server.Start();  // listen
                Console.WriteLine("Listening on port " + port);
                Console.WriteLine("Waiting for connections...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    ThreadPool.QueueUserWorkItem(this.clientHandler, client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }
    }
}
