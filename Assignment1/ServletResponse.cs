using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
	public class ServletResponse
	{
		public ServletResponse() { }
		List<byte> resBytes = new List<byte>();

		public void writeToResponse(string resString)
		{
			byte[] resBytes = Encoding.ASCII.GetBytes(resString);
			foreach (byte resByte in resBytes)
			{
				this.resBytes.Add(resByte);
			}
		}

		public List<byte> getResponse()
		{
			return this.resBytes;
		}
	}
}
