using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class EmptyAppVersionHandler : AppVersionHandler
	{
		internal override bool IsValid(HttpRequestMessage request)
		{
			return true;
		}
	}
}