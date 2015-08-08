using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public abstract class GenericPipeline<T> : IPipeline<T>
	{
		public T Execute(T param)
		{
			return GetFilters().Aggregate(param, (p, f) => f.Execute(p));
		}

		protected abstract IEnumerable<IPipelineFilter<T>> GetFilters();
	}

	public abstract class GenericPipeline<T, U1> : IPipeline<T, U1>
	{
		public T Execute(T param, U1 param1)
		{
			return GetFilters().Aggregate(param, (p, f) => f.Execute(param, param1));
		}

		protected abstract IEnumerable<IPipelineFilter<T, U1>> GetFilters();
	}

	public abstract class GenericPipeline<T, U1, U2> : IPipeline<T, U1, U2>
	{
		public T Execute(T param, U1 param1, U2 param2)
		{
			return GetFilters().Aggregate(param, (p, f) => f.Execute(param, param1, param2));
		}

		protected abstract IEnumerable<IPipelineFilter<T, U1, U2>> GetFilters();
	}
}