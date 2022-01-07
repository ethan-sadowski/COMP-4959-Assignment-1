using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
namespace Assignment1
{
    public class ServerThread
    {
        Socket cls = null;
        public ServerThread(Socket socket) { this.cls = socket; }

        void httpCall(ServletRequest req, ServletResponse res)
        {
            // Reflection kinda ???
            Servlet myUploadServlet = new UploadServlet();
            Type myTypeObj = myUploadServlet.GetType();
            MethodInfo myGetInfo = myTypeObj.GetMethod("doGet");
            MethodInfo myPostInfo = myTypeObj.GetMethod("doPost");
            MethodInfo myCustomInfo = myTypeObj.GetMethod("doCustom");

            object[] mParam = new object[] {req, res};

            if (req.getHeader("Method") == "GET")
            {
                myGetInfo.Invoke(myUploadServlet, mParam);
            }
            else if (req.getHeader("Method") == "POST")
            {
                myPostInfo.Invoke(myUploadServlet, mParam);
            } else
            {
                myCustomInfo.Invoke(myUploadServlet, mParam);
            }
        }
        public void threadMethod()
        {
            Byte[] bytesReceived = new Byte[1];
            List<byte> byteList = new List<byte>();
            while (true)
            {
                int nextBytes = cls.Receive(bytesReceived, bytesReceived.Length, 0);
                if (cls.Available == 0)
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
