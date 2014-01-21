using System.Collections.Generic;
using System.Web.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Policies
{
	public class EnvironmentErrorDetailPolicyProvider : IErrorDetailPolicyProvider
	{
		public IDictionary<string, IncludeErrorDetailPolicy> GetPolicies()
		{
			return new Dictionary<string, IncludeErrorDetailPolicy>
			{
				{ Environment.Local, IncludeErrorDetailPolicy.Always },
				{ Environment.Development, IncludeErrorDetailPolicy.Always },
				{ Environment.Staging, IncludeErrorDetailPolicy.Always },
				{ Environment.Production, IncludeErrorDetailPolicy.LocalOnly },
			};
		}
	}
}