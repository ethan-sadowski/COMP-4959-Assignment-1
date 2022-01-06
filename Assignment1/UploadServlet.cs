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

		public void doPost(ServletRequest request, ServletResponse response)
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "../../../pictures/");
            Console.WriteLine(dir.GetFiles().Length);
            Console.WriteLine(dir.FullName);
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
            response.writeToResponse(bodyStart + files + bodyEnd);
        }
	}
}
