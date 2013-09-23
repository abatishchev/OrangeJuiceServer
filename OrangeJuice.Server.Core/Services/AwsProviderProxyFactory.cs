using System;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProviderProxyFactory : ProxyFactoryBase<IAwsProvider>
	{
		public AwsProviderProxyFactory(Func<IAwsProvider> func)
			: base(func)
		{
		}
	}
}