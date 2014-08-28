using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class QueryAppVersionValidator : IValidator<HttpRequestMessage>
	{
		private readonly Version _appVersion;

		public QueryAppVersionValidator(Version appVersion)
		{
			_appVersion = appVersion;
		}

		public bool IsValid(HttpRequestMessage request)
		{
			return GetRules(request).All(b => b);
		}

		private IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			string appVer = request.RequestUri.ParseQueryString()["appVer"];

			Version version;
			yield return Version.TryParse(appVer, out version);

			yield return version == _appVersion;
		}
	}
}