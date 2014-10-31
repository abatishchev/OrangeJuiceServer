using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public abstract class ObjectPipeline : IPipeline
	{
		public object Execute(object param)
		{
			return GetFilters().Aggregate(param, (p, f) => f.Execute(p));
		}

		protected abstract IEnumerable<IPipelineFilter> GetFilters();
	}
}