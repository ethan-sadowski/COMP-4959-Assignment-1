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
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "../../../../../../Assignment1/pictures/");
            string fileName = dir.GetFiles()[0].FullName;
            byte[] fileData = File.ReadAllBytes(fileName);

            HttpMethod custom = new HttpMethod("CUSTOM");
            string formEndString = "------WebKitFormBoundary";
            int contentLength = fileData.Length;
            string requestHeader = "CUSTOM\r\nContent-Length: " + contentLength + "\n\r\n\r\n\r\n";


            byte[] fileDataByte = Encoding.ASCII.GetBytes(requestHeader);
            byte[] clientData = new byte[requestHeader.Length + fileData.Length + formEndString.Length];
            byte[] formEnd = Encoding.ASCII.GetBytes(formEndString);

            fileDataByte.CopyTo(clientData, 0);
            fileData.CopyTo(clientData, requestHeader.Length);
            formEnd.CopyTo(clientData, requestHeader.Length + fileData.Length);
            Console.WriteLine(fileData.Length);
            Console.WriteLine(clientData.Length - requestHeader.Length);
            //Console.WriteLine(Encoding.Default.GetString(clientData));
            clientSocket.Connect(ipEnd);
            clientSocket.Send(clientData);
            clientSocket.Close();
        }
    }
}