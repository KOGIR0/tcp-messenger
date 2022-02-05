using System;
using System.Text;
using System.Net.Sockets;

namespace Client.Connection
{
    class MessangerClient
    {

        private Guid id;
        private Guid code;

        public MessangerClient()
        {
            this.id = Guid.NewGuid();
        }
        public void RunClient()
        {
            // registration
            const int port = 8000;
            const string server = "127.0.0.1";
            this.Register(port, server);

            // send message
            this.StartMessaging(8001, server);
        }

        public void StartMessaging(int port, string server)
        {
            Console.WriteLine("Enter 'DISCONNECT' to disconnect");
            Console.Write("Enter message: ");
            string input = Console.ReadLine();
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(server, port);

                    using (NetworkStream stream = client.GetStream())
                    {
                        do
                        {
                            string fullMessage = input + " " + this.id + " " + this.code;
                            // send message
                            this.Write(stream, fullMessage);

                            // recive message
                            string response = this.ReciveMessage(stream);
                            Console.WriteLine($"Recived message by: {response}");

                            Console.Write("Enter message: ");
                            input = Console.ReadLine();
                        } while (input != "DISCONNECT" && client.Connected);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("End of request...");
        }

        public void Register(int port, string server)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(server, port);

                    if (!client.Connected)
                    {
                        return;
                    }

                    using (NetworkStream stream = client.GetStream())
                    {

                        // send message
                        this.id = Guid.NewGuid();
                        Console.WriteLine("Send message: " + this.id.ToString());
                        this.Write(stream, this.id.ToString());

                        // recive message
                        string response = this.ReciveMessage(stream);
                        Console.WriteLine($"Recived message by {this.id}: {response}");

                        this.code = new Guid(response.ToString());
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("End of request...");
        }

        private void Write(NetworkStream stream, string message)
        {
            stream.Write(Encoding.Default.GetBytes(message), 0, Encoding.Default.GetBytes(message).Length);
        }

        private string ReciveMessage(NetworkStream stream)
        {
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                byte[] userMessage = new byte[2048];
                int n = stream.Read(userMessage, 0, userMessage.Length);
                stringBuilder.Append(Encoding.Default.GetString(userMessage, 0, n));
            } while (stream.DataAvailable);

            return stringBuilder.ToString();
        }
    }
}
