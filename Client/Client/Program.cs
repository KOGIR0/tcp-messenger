using System.Threading;

using Client.Connection;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MessangerClient client = new MessangerClient();
            client.RunClient();
        }
    }
}
