using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections;

namespace Assignment1
{
	class UploadServlet : Servlet
	{
        public UploadServlet() { }

        void savePicture(List<byte> pictureData)
        {
            System.IO.File.WriteAllBytes("../../pictures/picture.jpg", pictureData.ToArray());
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
            for (int i = imgIndex + 1; i < imgEnd - 1; i++)
            {
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
            Console.WriteLine(req.getRequestString());
            List<byte> pictureData = parsePicture(req);
            savePicture(pictureData);
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "../../../pictures/");
            string bodyStart =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/html\r\n\r\n" +
                "<!DOCTYPE html>" +
                "<html><head><title>File Upload Form</title></head>" +
                "<body><h1>Current Photos</h1>" +
                "<ul>";
            string files = "";
            foreach (FileInfo item in dir.GetFiles())
            {
                files += "<li>" + item.Name + "</li>";
            }
            string bodyEnd = "</ul></body></html>\r\n";
            res.writeToResponse(bodyStart + files + bodyEnd);
        }
        public void doCustom(ServletRequest req, ServletResponse res)
        {
            Console.WriteLine("doCustom");
            //Console.WriteLine(req.getRequestString());
            List<byte> pictureData = parsePicture(req);
            savePicture(pictureData);
        }

    }
}
