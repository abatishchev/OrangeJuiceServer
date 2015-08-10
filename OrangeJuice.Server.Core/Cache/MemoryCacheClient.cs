using System;
using System.Runtime.Caching;

namespace OrangeJuice.Server.Cache
{
	public sealed class MemoryCacheClient : ICacheClient
	{
		private readonly ObjectCache _cache;

		public MemoryCacheClient(ObjectCache cache)
		{
			_cache = cache;
		}

		public T AddOrGetExisting<T>(string key, Func<T> valueFactory)
		{
			return AddOrGetExisting(key, valueFactory, () => new CacheItemPolicy());
		}

		public T AddOrGetExisting<T>(string key, Func<T> valueFactory, Func<CacheItemPolicy> policyFactory)
		{
			var newValue = new Lazy<T>(valueFactory);
			var policy = policyFactory();
			var value = (Lazy<T>)_cache.AddOrGetExisting(key, newValue, policy);
			return (value ?? newValue).Value;
		}
	}
}