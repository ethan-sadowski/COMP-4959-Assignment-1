﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Assignment1
{
    public class ServerThread
    {
        Socket cls = null;
        public ServerThread(Socket socket) { this.cls = socket; }

        void httpCall(ServletRequest req, ServletResponse res)
        {
            Servlet httpServlet = new UploadServlet();
            if (req.getHeader("Method") == "GET")
            {
                httpServlet.doGet(req, res);
            }
            else 
            {
                httpServlet.doPost(req, res);
            }
        }
        public void threadMethod()
        {
            Byte[] bytesReceived = new Byte[1];
            List<byte> byteList = new List<byte>();
            while (true)
            {
                int nextBytes = cls.Receive(bytesReceived, bytesReceived.Length, 0);
                if (cls.Available == 0 || (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0'))
                {
                    break;
                }
                byteList.Add(bytesReceived[0]);
            }
            ServletRequest req = new ServletRequest(byteList);
            ServletResponse res = new ServletResponse();
            httpCall(req, res);
            cls.Send(res.getResponse().ToArray(), res.getResponse().ToArray().Count(), 0);
            cls.Close();
        }
    }
}