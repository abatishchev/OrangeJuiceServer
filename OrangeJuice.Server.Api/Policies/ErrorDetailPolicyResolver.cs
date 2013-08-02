using System;
using System.Collections.Generic;
using System.Web.Http;

using OrangeJuice.Server.Configuration;

using Environment = OrangeJuice.Server.Configuration.Environment;

namespace OrangeJuice.Server.Api.Policies
{
	internal sealed class ErrorDetailPolicyResolver
	{
		private static readonly Lazy<IDictionary<string, IncludeErrorDetailPolicy>> _defaultPolicies = new Lazy<IDictionary<string, IncludeErrorDetailPolicy>>(GetDefaultPolicies);

		private readonly IEnvironmentProvider _environmentProvider;
		private readonly IDictionary<string, IncludeErrorDetailPolicy> _policies;

		public ErrorDetailPolicyResolver(IEnvironmentProvider environmentProvider)
			: this(environmentProvider, _defaultPolicies.Value)
		{
		}

		internal ErrorDetailPolicyResolver(IEnvironmentProvider environmentProvider, IDictionary<string, IncludeErrorDetailPolicy> policies)
		{
			if (environmentProvider == null)
				throw new ArgumentNullException("environmentProvider");

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
			try
			{
				return _policies[environment];
			}
			catch (KeyNotFoundException ex)
			{
				throw new InvalidOperationException("Current environment is not supported", ex);
			}
		}
	}
}