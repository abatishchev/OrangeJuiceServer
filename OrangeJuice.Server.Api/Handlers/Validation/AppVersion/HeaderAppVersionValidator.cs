using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class HeaderAppVersionValidator : RulesValidator<HttpRequestMessage>
	{
		private readonly Version _appVersion;

		public HeaderAppVersionValidator(Version appVersion)
		{
			_appVersion = appVersion;
		}

		protected override IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			IEnumerable<string> values;
			yield return request.Headers.TryGetValues("AppVer", out values);

			Version version;
			yield return Version.TryParse(values.FirstOrDefault(), out version);

			yield return version == _appVersion;
		}
	}
}