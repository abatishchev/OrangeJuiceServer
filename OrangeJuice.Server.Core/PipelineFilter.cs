using System;
using System.Linq;

namespace OrangeJuice.Server
{
	public class PipelineFilter<T> : IPipelineFilter<T>
	{
		private readonly Func<T, T> _func;

		public PipelineFilter(Func<T, T> func)
		{
			_func = func;
		}

		public T Execute(T param)
		{
			return _func(param);
		}
	}

	public static class PipelineFilter
	{
		public static PipelineFilter<T> Create<T>(Func<T, T> func)
		{
			return new PipelineFilter<T>(func);
		}
	}
}