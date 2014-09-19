using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public sealed class ThrottlingHttpClient : HttpClient
	{
		#region Fields
		private readonly TaskFactory _taskFactory;

		#endregion

		#region Ctor
		public ThrottlingHttpClient(TaskFactory taskFactory)
		{
			_taskFactory = taskFactory;
		}

		#endregion

		#region IHttpClient members
		public override Task<string> GetStringAsync(Uri url)
		{
			return _taskFactory.StartNew(async () => await base.GetStringAsync(url)).Unwrap();
		}
		#endregion
	}
}