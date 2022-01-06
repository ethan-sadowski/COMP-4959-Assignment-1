using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
	public class ServletRequest
	{
		List<byte> reqBytes = new List<byte>();
		Dictionary<string, string> headers = new Dictionary<string, string>();
		public ServletRequest(List<byte> reqBytes)
		{
			this.reqBytes = reqBytes;
			parseHeaders();
		}

		void addHeader(string key, string value)
		{
			this.headers.Add(key, value);
		}

		void parseHeaders()
		{
			string nextString = "";
			bool run = true;
			for (int i = 0; i < this.reqBytes.Count; i++) 
			{
				char nextChar = (char)this.reqBytes[i];
				if (nextString == "GET")
				{
					addHeader("Method", nextString);
					nextString = "";
				}
				else if (nextString == "POST")
				{
					addHeader("Method", nextString);
					nextString = "";
				}
				else if (nextString == "Content-Length:")
				{
					nextString = "";
					i++;
					while ((char)this.reqBytes[i] != ' ' && (char)this.reqBytes[i] != '\r' && (char)this.reqBytes[i] != '\n')
					{
						nextString += (char)this.reqBytes[i];
						Console.Write((char)this.reqBytes[i]);
						i++;
					}
					addHeader("Content-Length", nextString);
					nextString = "";
				}
				else if (nextString == "User-Agent:")
				{
					nextString = "";
					i++;
					while ((char)this.reqBytes[i] != ' ' && (char)this.reqBytes[i] != '\r' && (char)this.reqBytes[i] != '\n')
					{
						nextString += (char)this.reqBytes[i];
						i++;
					}
					addHeader("User-Agent", nextString);
					nextString = "";
				}
				else if (reqBytes[i] == '\n')
				{
					if (nextString.Length > 0 && nextString[0] != '\r') 
					{
						nextString = "";
					}
				}
				else
				{
					nextChar = (char) reqBytes[i];
					nextString += nextChar;

					/* may need to change back to \r\n\r\n? For a browser GET request the server only seems to recognize \r\n */
					if (nextString == "\r\n")
					{
						run = false;
						break;
					}
				}
			}
			/*foreach (string key in this.headers.Keys)
			{
				Console.WriteLine(key);
				Console.WriteLine(this.headers[key]);
			}*/
		}

		public List<byte> getRequestBytes()
		{
			return this.reqBytes;
		}

		public string getRequestString()
		{
			string requestString = "";
			foreach (byte b in this.reqBytes)
			{
				requestString += (char)b;
			}
			return requestString;
		}

		public string getHeader(string key)
		{
			return this.headers[key];
		}

	}
}
