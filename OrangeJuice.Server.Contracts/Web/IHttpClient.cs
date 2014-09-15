using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public interface IHttpClient
	{
		Task<string> GetStringAsync(Uri url);
	}
}