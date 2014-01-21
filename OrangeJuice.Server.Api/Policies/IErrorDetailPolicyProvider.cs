using System.Collections.Generic;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Policies
{
	public interface IErrorDetailPolicyProvider
	{
		IDictionary<string, IncludeErrorDetailPolicy> GetPolicies();
	}
}