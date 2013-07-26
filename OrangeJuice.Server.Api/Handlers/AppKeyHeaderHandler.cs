using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppKeyHeaderHandler : ValidatingDelegatingHandler
	{
		internal const string AppKeyHeaderName = "X-ApiKey";

		private readonly Guid _appKey;

		public AppKeyHeaderHandler(Guid appKey)
		{
			_appKey = appKey;
		}

		internal override HttpStatusCode ErrorCode
		{
			get { return HttpStatusCode.Forbidden; }
		}

		internal override bool IsValid(System.Net.Http.HttpRequestMessage request)
		{
			IEnumerable<string> values;
			Guid guid;
			return request.Headers.TryGetValues(AppKeyHeaderName, out values) &&
				Guid.TryParse(values.FirstOrDefault(), out guid) && guid == _appKey;
		}
	}
}