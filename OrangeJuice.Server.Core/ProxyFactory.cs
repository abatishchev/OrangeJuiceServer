using System;

namespace OrangeJuice.Server
{
	public sealed class ProxyFactory<T> : IFactory<T>
	{
		private readonly Func<T> _func;

		public ProxyFactory(Func<T> func)
		{
			_func = func;
		}

		public T Create()
		{
			return _func();
		}
	}
}