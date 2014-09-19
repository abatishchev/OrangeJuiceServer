using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public sealed class ThrottlingHttpClient : HttpClient
	{
		#region Fields
		private static readonly TaskFactory TaskFactory = CreateTaskFactory();

		private static TaskFactory CreateTaskFactory()
		{
			return new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.None, new ReactiveSampleScheduler());
		}
		#endregion

		#region Ctor
		public ThrottlingHttpClient()
		{
		}
		#endregion

		#region IHttpClient members
		public override Task<string> GetStringAsync(Uri url)
		{
			return TaskFactory.StartNew(async () => await base.GetStringAsync(url)).Unwrap();
		}
		#endregion
	}
}