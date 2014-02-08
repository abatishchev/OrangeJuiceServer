using System.Web.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Policies
{
	public sealed class ErrorDetailPolicyFactory : IFactory<IncludeErrorDetailPolicy>
	{
		#region Fields
		private readonly IErrorDetailPolicyProvider _detailPolicyProvider;
		private readonly IEnvironmentProvider _environmentProvider;
		#endregion

		#region Ctor
		public ErrorDetailPolicyFactory(IErrorDetailPolicyProvider detailPolicyProvider, IEnvironmentProvider environmentProvider)
		{
			_detailPolicyProvider = detailPolicyProvider;
			_environmentProvider = environmentProvider;
		}
		#endregion

		#region IFactory members
		public IncludeErrorDetailPolicy Create()
		{
			var policies = _detailPolicyProvider.GetPolicies();
			string environment = _environmentProvider.GetCurrentEnvironment();

			IncludeErrorDetailPolicy policy;
			if (!policies.TryGetValue(environment, out policy))
				return IncludeErrorDetailPolicy.Default;

			return policy;
		}
		#endregion
	}
}