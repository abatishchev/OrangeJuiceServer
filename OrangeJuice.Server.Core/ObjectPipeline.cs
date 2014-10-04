using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public class ObjectPipeline : IPipeline
	{
		private readonly ICollection<IPipelineFilter> _filters = new List<IPipelineFilter>();

		public object Execute(object param)
		{
			return _filters.Aggregate(param, (p, f) => f.Execute(p));
		}

		protected virtual IEnumerable<IPipelineFilter> GetFilters()
		{
			return _filters;
		}

		public IPipeline Register<TParam, TResult>(IPipelineFilter<TParam, TResult> filter)
		{
			_filters.Add(filter);
			return this;
		}
	}
}