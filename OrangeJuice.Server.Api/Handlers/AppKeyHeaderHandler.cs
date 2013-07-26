using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppKeyHeaderHandler : DelegatingHandler
	{
		internal const string AppKeyHeaderName = "X-ApiKey";

		private readonly Guid _appKey;

		public AppKeyHeaderHandler(Guid appKey)
		{
			_appKey = appKey;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (IsValid(request))
				return base.SendAsync(request, cancellationToken);

			HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
			var tsc = new TaskCompletionSource<HttpResponseMessage>();
			tsc.SetResult(response);
			return tsc.Task;
		}

		internal bool IsValid(HttpRequestMessage request)
		{
			IEnumerable<string> values;
			Guid guid;
			return request.Headers.TryGetValues(AppKeyHeaderName, out values) &&
				Guid.TryParse(values.FirstOrDefault(), out guid) && guid == _appKey;
		}
	}
}