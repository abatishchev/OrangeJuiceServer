using System;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProviderProxyFactory : IAwsProviderFactory
	{
		private readonly Func<IAwsProvider> _func;

		public AwsProviderProxyFactory(Func<IAwsProvider> func)
		{
			if (func == null)
				throw new ArgumentNullException("func");
			_func = func;
		}

		public IAwsProvider Create()
		{
			return _func();
		}
	}

	public sealed class ProxyFactory<T>
	{
		private readonly Func<T> _func;

		public ProxyFactory(Func<T> func)
		{
			if (func == null)
				throw new ArgumentNullException("func");
			_func = func;
		}

		public T Create()
		{
			return _func();
		}
	}
}