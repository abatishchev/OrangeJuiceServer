using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using OrangeJuice.Server.Validation;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AcceptHeaderValidator : RulesValidator<HttpRequestMessage>
	{
		private readonly Version _appVersion;

		public AcceptHeaderValidator(Version appVersion)
		{
			_appVersion = appVersion;
		}

		protected override IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			IEnumerable<string> values;
			yield return request.Headers.TryGetValues("Accept", out values);
			yield return values.Any(v =>
				String.Equals(
					String.Format("application/vnd.orangejuice.v{0}+json", _appVersion),
					v,
					StringComparison.OrdinalIgnoreCase));
		}
	}
}