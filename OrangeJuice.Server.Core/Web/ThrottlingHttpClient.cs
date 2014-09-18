using System;
using System.Threading.Tasks;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Web
{
	public sealed class ThrottlingHttpClient : HttpClient
	{
		private readonly IAwsApiProvider _provider;

		public ThrottlingHttpClient(IAwsApiProvider provider)
		{
			_provider = provider;
		}

		public override Task<string> GetStringAsync(Uri url)
		{
			return _provider.ScheduleRequest(base.GetStringAsync(url));
		}
	}
}