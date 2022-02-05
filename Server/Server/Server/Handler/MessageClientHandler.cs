using System;
using System.Net.Sockets;
using Server.Server.Storage;
using Server.Server.Log;

namespace Server.Server.Handler
{
    /// <summary>
    /// Handler sends message from client to server
    /// </summary>
    class MessageClientHandler: IClientHandler
    {
        private Guid userId;
        private NetworkStream stream;

        public override void HandleClient(object o)
        {
            Console.WriteLine("Client connected");
            using (TcpClient client = o as TcpClient)
            {
                using (this.stream = client.GetStream())
                {
                    while (client.Connected)
                    {
                        // try recive message
                        try
                        {
                            string userMessage = this.ReciveMessage(stream);
                            if (userMessage.Length == 0)
                            {
                                // Check connection
                                // if client disconnected client.Connected will be set to false
                                this.SendMessage(stream, "Connection check");
                                continue;
                            }
                            this.ReplyToMessage(userMessage);
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }

                    Logger.Log($"User {this.userId} disconnected");
                }
            }
        }

        /// <summary>
        /// Send response to user
        /// </summary>
        /// <param name="userMessage">message in format: message userId userCode</param>
        private void ReplyToMessage(string userMessage)
        {
            Guid userCode = new Guid();

            try
            {
                string[] messageSplit = userMessage.Split(" ");
                this.userId = new Guid(messageSplit[messageSplit.Length - 2]);
                userCode = new Guid(messageSplit[messageSplit.Length - 1]);

                string[] result = new string[messageSplit.Length - 2];
                Array.Copy(messageSplit, 0, result, 0, messageSplit.Length - 2);
                userMessage = String.Join(' ', result);
            } catch (Exception e)
            {
                this.SendMessage(stream, "Error in user message: " + e.Message);
            }

            if (Store.ContainsKey(userId))
            {
                Guid code = Store.Get(userId);
                if (code == userCode)
                {
                    Logger.Log($"Message accepted from {userId}: {userMessage}");
                    this.SendMessage(stream, "Message accepted from " + userId);
                }
                else
                {
                    this.SendMessage(stream, "Error wrong code from " + userId);
                }
            }
            else
            {
                this.SendMessage(stream, "Error user not regestered: " + userId);
            }
        }
    }
}
