using System;
using System.IO;
using System.Threading.Tasks;




namespace OrangeJuice.Server.Web

{

	public interface IHttpClient
	{

		Task<Stream> ParseQueryString(Uri url);
	}

}