using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class EmptyAppKeyHandler : AppKeyHandlerBase
	{
		internal override bool IsValid(HttpRequestMessage request)
		{
			return true;
		}
	}
}