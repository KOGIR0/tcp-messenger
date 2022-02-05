using System;
using System.Net.Sockets;
using Server.Server.Storage;

namespace Server.Server.Handler
{
    /// <summary>
    /// Handler creates code for client
    /// </summary>
    class CodeClientHandler: IClientHandler
    {
        public override void HandleClient(object o)
        {
            Console.WriteLine("Client connected");
            using (TcpClient client = o as TcpClient)
            {
                using (NetworkStream stream = client.GetStream())
                {
                    string userMessage = this.ReciveMessage(stream);
                    Guid guid;
                    try
                    {
                        // convert accepted message to Guid
                        guid = new Guid(userMessage);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Error converting message to Guid");
                        this.SendMessage(stream, "Wrong Guid format");
                        return;
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("Error converting message to Guid");
                        this.SendMessage(stream, "Guid overflow");
                        return;
                    }

                    // generate user code and save it
                    Guid code = Guid.NewGuid();
                    Store.Add(guid, code);

                    // send code to user
                    this.SendMessage(stream, code.ToString());
                    Console.WriteLine("Send message: {0}", code.ToString());
                }
            }
        }
    }
}
