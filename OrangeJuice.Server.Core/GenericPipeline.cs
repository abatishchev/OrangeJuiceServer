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

		protected abstract IEnumerable<IPipelineFilter<T, T>> GetFilters();
	}
}