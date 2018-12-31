using System;
using Server64;

namespace ConsoleAppServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServer server = new MyServer();
            server.Server();
        }
    }
}
