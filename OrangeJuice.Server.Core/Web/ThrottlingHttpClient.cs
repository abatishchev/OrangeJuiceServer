using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Threading;

namespace OrangeJuice.Server.Web
{
	public sealed class ThrottlingHttpClient : IHttpClient
	{
		#region Fields
		private readonly IHttpClient _httpClient;
		private readonly IRequestScheduler _scheduler;

		#endregion

		#region Ctor
		public ThrottlingHttpClient(IHttpClient httpClient, IRequestScheduler scheduler)
		{
			_httpClient = httpClient;
			_scheduler = scheduler;
		}
		#endregion

		#region IHttpClient members
		public Task<string> GetStringAsync(Uri url)
		{
			return _scheduler.ScheduleRequest(() => _httpClient.GetStringAsync(url));
		}
		#endregion
	}
}