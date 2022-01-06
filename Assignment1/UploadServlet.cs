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
            System.IO.File.WriteAllBytes("picture.jpg", pictureData.ToArray());
        }
        List<byte> parsePicture(ServletRequest req)
        {
            int byteLength = req.getRequestBytes().Count;
            int imgSize = Int32.Parse(req.getHeader("Content-Length"));
            List<byte> pictureData = new List<byte>();
            int bytesRead = 0;
            byte[] imageBuffer = new byte[imgSize];
            byte[] reqBytes = req.getRequestBytes().ToArray();
            int webkitCount = 0;
            string s = req.getRequestString();
            int imgIndex = s.IndexOf("\n\r", s.IndexOf("\r\n\r\n") + 4) + 2;
            int imgEnd = s.IndexOf("------WebKitFormBoundary", imgIndex);
            List<byte> imageBytes = new List<byte>();
            Console.WriteLine(imgIndex);
            Console.WriteLine(imgEnd);
            for (int i = imgIndex + 1; i < imgEnd - 1; i++)
            {
                Console.Write((char) reqBytes[i]);
                imageBytes.Add(reqBytes[i]);
            }

            return imageBytes;
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
