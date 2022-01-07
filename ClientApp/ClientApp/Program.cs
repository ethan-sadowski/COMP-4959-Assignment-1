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

            bool fileFound = false;
            string fileName = "";
            while (!fileFound)
            {
                Console.WriteLine("Enter the name of the picture you would like to upload.\n Please ensure the picture is located in the same root directory as the console client.");
                string readFileName = Console.ReadLine();
                if (File.Exists(Directory.GetCurrentDirectory() + "\\" + readFileName))
                {
                    fileFound = true;
                    fileName = readFileName;
                    break;
                }
            
            }

            byte[] image = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\" + fileName);

            
            string formEndString = "------WebKitFormBoundary";
            int contentLength = image.Length;
            
            Console.WriteLine("Enter the date:");
            string date = Console.ReadLine();
            Console.WriteLine("Enter a caption:");
            string caption = Console.ReadLine();

            string requestHeader = "CUSTOM\r\nContent-Length: " + contentLength
                + "\r\nName: " + caption + date + fileName 
                + "\n\r\n\r\n\r\n";

            byte[] fileDataByte = Encoding.ASCII.GetBytes(requestHeader);
            byte[] clientData = new byte[requestHeader.Length + image.Length + formEndString.Length];
            byte[] formEnd = Encoding.ASCII.GetBytes(formEndString);

            fileDataByte.CopyTo(clientData, 0);
            image.CopyTo(clientData, requestHeader.Length);
            formEnd.CopyTo(clientData, requestHeader.Length + image.Length);
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