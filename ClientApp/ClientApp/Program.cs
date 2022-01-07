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
            FileInfo file = dir.GetFiles()[0];
            string fileName = file.FullName;
            byte[] fileData = File.ReadAllBytes(fileName);
            
            string formEndString = "------WebKitFormBoundary";
            int contentLength = fileData.Length;
            Console.WriteLine("Enter the date:");
            string date = Console.ReadLine();
            Console.WriteLine("Enter a caption:");
            string caption = Console.ReadLine();

            string requestHeader = "CUSTOM\r\nContent-Length: " + contentLength
                + "\r\nName: " + caption + date + file.Name
                + "\n\r\n\r\n\r\n";

            byte[] fileDataByte = Encoding.ASCII.GetBytes(requestHeader);
            byte[] clientData = new byte[requestHeader.Length + fileData.Length + formEndString.Length];
            byte[] formEnd = Encoding.ASCII.GetBytes(formEndString);

            fileDataByte.CopyTo(clientData, 0);
            fileData.CopyTo(clientData, requestHeader.Length);
            formEnd.CopyTo(clientData, requestHeader.Length + fileData.Length);
            clientSocket.Connect(ipEnd);
            int res = clientSocket.Send(clientData);
            string a = "";
            if (clientSocket.Connected)
            {
                Byte[] bytesReceived = new Byte[1];
                while (true)
                {
                    if ((clientSocket.Receive(bytesReceived, bytesReceived.Length, 0) == 0) ||
                              (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0'))
                    {
                        break;
                    }
                    a += Encoding.ASCII.GetString(bytesReceived, 0, 1);
                }
            }
            Console.WriteLine(a);
            Console.Read();
        }
    }
}