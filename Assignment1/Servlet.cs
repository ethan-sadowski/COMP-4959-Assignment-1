using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
	interface Servlet
	{
		void doGet(ServletRequest request, ServletResponse response);
		void doPost(ServletRequest request, ServletResponse response);
		void doCustom(ServletRequest request, ServletResponse response);
	}
}
