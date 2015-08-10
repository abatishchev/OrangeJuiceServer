using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Cache;

namespace OrangeJuice.Server.Configuration
{
	public class CachingAwsOptionsProvider : IOptionsProvider<AwsOptions>
	{
		private readonly IOptionsProvider<AwsOptions> _optionsProvider;
		private readonly ICacheClient _cacheClient;

		public CachingAwsOptionsProvider(IOptionsProvider<AwsOptions> optionsProvider, ICacheClient cacheClient)
		{
			_optionsProvider = optionsProvider;
			_cacheClient = cacheClient;
		}

		public Task<IEnumerable<AwsOptions>> GetOptions()
		{
			return _cacheClient.AddOrGetExisting(AwsOptions.CacheKey, () => _optionsProvider.GetOptions());
		}
	}
}