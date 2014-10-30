using System.Net.Http;

namespace OrangeJuice.Server.Web
{
	public interface IRequestMessageProvider
	{
		HttpRequestMessage CurrentMessage { get; }
	}
}