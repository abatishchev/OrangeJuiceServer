using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Threading;

namespace OrangeJuice.Server.Web
{
	public sealed class ThrottlingHttpClient : HttpClient
	{
		#region Fields
		private readonly IRequestScheduler _scheduler;

		#endregion

		#region Ctor
		public ThrottlingHttpClient(IRequestScheduler scheduler)
		{
			_scheduler = scheduler;
		}

		#endregion

		#region IHttpClient members
		public override Task<string> GetStringAsync(Uri url)
		{
			return _scheduler.ScheduleRequest(() => base.GetStringAsync(url));
		}
		#endregion
	}
}