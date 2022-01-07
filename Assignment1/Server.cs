using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace Assignment1
{

    class Server
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 8888; IPAddress address = IPAddress.Parse("127.0.0.1");
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                s.Bind(ipe); s.Listen(10);
                while (true)
                {
                    Socket cls = s.Accept();
                    ServerThread serverThread = new ServerThread(cls);
                    Thread thread = new Thread(new ThreadStart(serverThread.threadMethod));
                    thread.Start();
                }

                s.Close();
            }
            catch (SocketException e) 
            {
                Console.WriteLine("Socket exception: {0}", e);
            }
        }
    }
}
