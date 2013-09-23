using System;

namespace OrangeJuice.Server
{
	public abstract class ProxyFactoryBase<T> : IFactory<T>
	{
		private readonly Func<T> _func;

		protected ProxyFactoryBase(Func<T> func)
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