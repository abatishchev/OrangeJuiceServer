﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppKeyQueryHandler : DelegatingHandler
	{
		internal const string AppKeySegmentName = "appKey";

		private readonly Guid _appKey;

		public AppKeyQueryHandler(Guid appKey)
		{
			_appKey = appKey;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
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
			var query = request.RequestUri.ParseQueryString();
			string appKey = query[AppKeySegmentName];

			Guid guid;
			return Guid.TryParse(appKey, out guid) &&
				guid == _appKey;
		}
	}
}