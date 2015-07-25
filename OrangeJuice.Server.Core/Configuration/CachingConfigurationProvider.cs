using System;
using System.Runtime.Caching;

namespace OrangeJuice.Server.Configuration
{
	public sealed class CachingConfigurationProvider : IConfigurationProvider
	{
		private readonly IConfigurationProvider _configurationProvider;
		private readonly MemoryCache _cache;

		public CachingConfigurationProvider(IConfigurationProvider configurationProvider, MemoryCache cache)
		{
			_configurationProvider = configurationProvider;
			_cache = cache;
		}

		public string GetValue(string key)
		{
			return RetrieveAndCache(key, () => _configurationProvider.GetValue(key), CreatePolicy);
		}

		private static CacheItemPolicy CreatePolicy()
		{
			return new CacheItemPolicy();
		}

		private T RetrieveAndCache<T>(string key, Func<T> valueFactory, Func<CacheItemPolicy> policyFactory)
		{
			var newValue = new Lazy<T>(valueFactory);
			var policy = policyFactory();
			var value = (Lazy<T>)_cache.AddOrGetExisting(key, newValue, policy);
			return (value ?? newValue).Value;
		}
	}
}