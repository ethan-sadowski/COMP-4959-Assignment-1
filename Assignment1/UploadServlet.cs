using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assignment1 {
    class UploadServlet : Servlet {
        public UploadServlet() {
        }

        void savePicture(List<byte> pictureData, string filename) {
            File.WriteAllBytes("../../pictures/" + filename, pictureData.ToArray());
        }
        void parsePicture(ServletRequest req) {
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

            int filenameIndex = s.IndexOf("filename=\"") + 10;
            int filenameEnd = s.IndexOf("\"\r\n", filenameIndex);

            string filename = s.Substring(filenameIndex, filenameEnd - filenameIndex);


            // Add 13 (9 for "caption" + 4 for \r\n\r\n)
            int capIndex = s.IndexOf("\"caption\"", imgEnd) + 13;
            int capEnd = s.IndexOf("------WebKitFormBoundary", capIndex) - 2;
            string cap = s.Substring(capIndex, capEnd - capIndex);

            // Add 10 (6 for "caption" + 4 for \r\n\r\n)
            int dateIndex = s.IndexOf("\"date\"", capEnd) + 10;
            int dateEnd = s.IndexOf("------WebKitFormBoundary", dateIndex) - 2;
            string date = s.Substring(dateIndex, dateEnd - dateIndex);

            Console.WriteLine(s);
            List<byte> imageBytes = new List<byte>();
            for(int i = imgIndex + 1; i < imgEnd - 1; i++) {
                imageBytes.Add(reqBytes[i]);
            }
            string picname = cap + date + filename;
            savePicture(imageBytes, picname);
        }
        public void doGet(ServletRequest req, ServletResponse res) {
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

        public void doPost(ServletRequest req, ServletResponse res) {
            parsePicture(req);
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "../../../pictures/");
            string bodyStart =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/html\r\n\r\n" +
                "<!DOCTYPE html>" +
                "<html><head><title>File Upload Form</title></head>" +
                "<body><h1>Current Photos</h1>" +
                "<ul>";
            string files = "";
            foreach(FileInfo item in dir.GetFiles()) {
                files += "<li>" + item.Name + "</li>";
            }
            string bodyEnd = "</ul></body></html>\r\n";
            res.writeToResponse(bodyStart + files + bodyEnd);
        }
        public string doCustom(ServletRequest req, ServletResponse res)
        {
            List<byte> pictureData = parseCustomPicture(req);
            saveCustomPicture(pictureData);
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "../../../pictures/");
            // Use NewtonSoft if we really care about serializing/deserializing with a package.
            // For our purposes had coding it works the same. It just goes to a String anyways.
            //Newtonsoft.Json json = 

            string bodyStart = "{\n";
            string files = "";
            foreach (FileInfo item in dir.GetFiles())
            {
                files += "  \"" + item.Name + "\": \"" + item.CreationTime +"\",\n";
            }
            string bodyEnd = "}";
            return bodyStart + files + bodyEnd;
        }

        void saveCustomPicture(List<byte> pictureData)
        {
            System.IO.File.WriteAllBytes("../../pictures/picture.jpg", pictureData.ToArray());
        }

        //Parses custom requests
        List<byte> parseCustomPicture(ServletRequest req)
        {
            
            int imgSize = Int32.Parse(req.getHeader("Content-Length"));
            byte[] reqBytes = req.getRequestBytes().ToArray();
            string s = req.getRequestString();

            int imgIndex = s.IndexOf("\n\r\n\r\n\r\n")+6;
            int imgEnd = s.IndexOf("------WebKitFormBoundar", imgIndex);

            List<byte> imageBytes = new List<byte>();
            for (int i = imgIndex + 1; i < imgEnd - 1; i++)
            {
                imageBytes.Add(reqBytes[i]);
            }
            return imageBytes;

        }
    }
}
