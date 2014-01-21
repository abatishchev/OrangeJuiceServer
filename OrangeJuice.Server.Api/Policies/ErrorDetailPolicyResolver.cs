using System;
using System.Web.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Policies
{
	public sealed class ErrorDetailPolicyResolver
	{
		#region Fields
		private readonly IErrorDetailPolicyProvider _detailPolicyProvider;
		private readonly IEnvironmentProvider _environmentProvider;
		#endregion

		#region Ctor
		public ErrorDetailPolicyResolver(IErrorDetailPolicyProvider detailPolicyProvider, IEnvironmentProvider environmentProvider)
		{
			_detailPolicyProvider = detailPolicyProvider;
			_environmentProvider = environmentProvider;
		}
		#endregion

		#region ErrorDetailPolicyResolver members
		public IncludeErrorDetailPolicy Resolve()
		{
			var policies = _detailPolicyProvider.GetPolicies();
			string environment = _environmentProvider.GetCurrentEnvironment();

			IncludeErrorDetailPolicy policy;
			if (!policies.TryGetValue(environment, out policy))
				throw new InvalidOperationException(String.Format("Evironment {0} is not supported", environment));

			return policy;
		}
		#endregion

	}
}