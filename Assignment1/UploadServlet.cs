using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
	class UploadServlet : Servlet
	{
        public UploadServlet() { }

        void savePicture(List<byte> pictureData)
        {

        }
        List<byte> parsePicture(ServletRequest req)
        {
            int byteLength = req.getRequestBytes().Count;
            int imgSize = Int32.Parse(req.getHeader("Content-Length"));
            List<byte> pictureData = new List<byte>();
            int bytesRead = 0;
            byte[] imageBuffer = new byte[imgSize];
            byte[] reqBytes = req.getRequestBytes().ToArray();
            for (int i = 0; i < byteLength; i++)
            {
                Console.WriteLine("Character: " + (char) reqBytes[i]);
            }
            return new List<byte>();
        }
		public void doGet(ServletRequest req, ServletResponse res)
		{
            string body =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/html\r\n\r\n" +
                "<!DOCTYPE html>" +
                "<html><head><title>File Upload Form</title></head>" +
                "<body><h1>Upload file</h1>" +
                "<form method=\"POST\" action=\"upload\" " +
                "enctype=\"multipart/form-data\">" +
                "<input type=\"file\" name=\"fileName\"/><br/><br/>" +
                "Caption: <input type=\"text\" name=\"caption\"/><br/><br/>" +
                "<br />" +
                "Date: <input type=\"date\" name=\"date\"/><br/><br/>" +
                "<br />" +
                "<input type=\"submit\" value=\"Submit\"/>" +
                "</form>" +
                "</body></html>\r\n";
            res.writeToResponse(body);
        }

		public void doPost(ServletRequest req, ServletResponse res)
		{
            List<byte> pictureData = parsePicture(req);
            savePicture(pictureData);

        }
    }
}
