using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 8888);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Console.WriteLine(Directory.GetCurrentDirectory());
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "../../../../../../Assignment1/pictures/");
            Console.WriteLine(dir.GetFiles()[0].FullName);
            string fileName = dir.GetFiles()[0].FullName;
            byte[] fileData = File.ReadAllBytes(fileName);

            HttpMethod custom = new HttpMethod("CUSTOM");
            Console.WriteLine("Custom: " + custom.ToString());
            int contentLength = fileData.Length;
            string requestHeader = "CUSTOM\r\nContent-Length: " + contentLength + "\n\r\n\r\n\r\n";
            byte[] fileDataByte = Encoding.ASCII.GetBytes(requestHeader);
            byte[] clientData = new byte[requestHeader.Length + fileData.Length];

            byte[] customByteArr = Encoding.ASCII.GetBytes(custom.ToString());

            fileDataByte.CopyTo(clientData, 0);
            fileData.CopyTo(clientData, requestHeader.Length);
            Console.WriteLine(fileData.Length);
            Console.WriteLine(clientData.Length - requestHeader.Length);
            //Console.WriteLine(Encoding.Default.GetString(clientData));
            clientSocket.Connect(ipEnd);
            clientSocket.Send(clientData);
            clientSocket.Close();
        }
    }
}