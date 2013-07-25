using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public class AppKeyHandler : DelegatingHandler
	{
		private const string AppKeySegmentName = "appKey";

		private readonly string _appKey;

		public AppKeyHandler(string appKey)
		{
			if (String.IsNullOrEmpty(appKey))
				throw new ArgumentNullException("appKey");
			_appKey = appKey;
		}

		protected override Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			if (!ValidateKey(request))
			{
				HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
				var tsc = new TaskCompletionSource<HttpResponseMessage>();
				tsc.SetResult(response);
				return tsc.Task;
			}
			return base.SendAsync(request, cancellationToken);
		}

		private bool ValidateKey(HttpRequestMessage message)
		{
			var query = message.RequestUri.ParseQueryString();
			string appKey = query[AppKeySegmentName];
			return appKey == _appKey;
		}
	}
}