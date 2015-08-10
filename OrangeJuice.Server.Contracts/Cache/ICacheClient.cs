using System;
using System.Runtime.Caching;

namespace OrangeJuice.Server.Cache
{
	public interface ICacheClient
	{
		T AddOrGetExisting<T>(string key, Func<T> valueFactory);

		T AddOrGetExisting<T>(string key, Func<T> valueFactory, Func<CacheItemPolicy> policyFactory);
	}
}