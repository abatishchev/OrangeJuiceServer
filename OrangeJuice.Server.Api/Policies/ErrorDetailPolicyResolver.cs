using System.Collections.Generic;
using System.Web.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Policies
{
	internal sealed class ErrorDetailPolicyResolver
	{
		private readonly IEnvironmentProvider _environmentProvider;
		private readonly IDictionary<string, IncludeErrorDetailPolicy> _policies;

		public ErrorDetailPolicyResolver(IEnvironmentProvider environmentProvider)
			: this(environmentProvider, GetDefaultPolicies())
		{
		}

		internal ErrorDetailPolicyResolver(IEnvironmentProvider environmentProvider, IDictionary<string, IncludeErrorDetailPolicy> policies)
		{
			if (environmentProvider == null)
				throw new System.ArgumentNullException("environmentProvider");

			_environmentProvider = environmentProvider;
			_policies = policies;
		}

		private static IDictionary<string, IncludeErrorDetailPolicy> GetDefaultPolicies()
		{
			return new Dictionary<string, IncludeErrorDetailPolicy>
			{
				{ Environment.Local, IncludeErrorDetailPolicy.Always },
				{ Environment.Development, IncludeErrorDetailPolicy.Always },
				{ Environment.Staging, IncludeErrorDetailPolicy.Always },
				{ Environment.Production, IncludeErrorDetailPolicy.LocalOnly },
			};
		}

		public IncludeErrorDetailPolicy Resolve()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			IncludeErrorDetailPolicy policy;

			if (!_policies.TryGetValue(environment, out policy))
				throw new System.InvalidOperationException("Current environment is not supported");
			return policy;
		}
	}
}