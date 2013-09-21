using System;

namespace OrangeJuice.Server.Web
{
	public sealed class HttpDocumentLoaderProxyFactory : IDocumentLoaderFactory
	{
		private readonly Func<IDocumentLoader> _func;

		public HttpDocumentLoaderProxyFactory(Func<IDocumentLoader> func)
		{
			if (func == null)
				throw new ArgumentNullException("func");
			_func = func;
		}

		public IDocumentLoader Create()
		{
			return _func();
		}
	}
}