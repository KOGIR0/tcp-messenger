using System.Text;
using System.Net.Sockets;

namespace Server.Server.Handler
{
    abstract class IClientHandler
    {
        public abstract void HandleClient(object o);

        public string ReciveMessage(NetworkStream stream)
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

        public void SendMessage(NetworkStream stream, string message)
        {
            byte[] data = Encoding.Default.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}
