using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<IEnumerable<string>> ItemSearch(string title);

		Task<XElement> ItemAttributes(string id);

		Task<XElement> ItemImages(string id);
	}
}