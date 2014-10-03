using System;

namespace OrangeJuice.Server
{
	public interface IPipelineFilter
	{
		object Execute(object param);
	}

	public interface IPipelineFilter<in TParam, out TResult> : IPipelineFilter
	{
		TResult Execute(TParam param);
	}

	public class PipelineFilter<TParam, TResult> : IPipelineFilter<TParam, TResult>
	{
		private readonly Func<TParam, TResult> _func;

		public PipelineFilter(Func<TParam, TResult> func)
		{
			_func = func;
		}

		object IPipelineFilter.Execute(object param)
		{
			return this.Execute((TParam)param);
		}

		public TResult Execute(TParam param)
		{
			return _func(param);
		}
	}
}
