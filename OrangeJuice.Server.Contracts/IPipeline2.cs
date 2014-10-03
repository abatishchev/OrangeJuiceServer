using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
    public interface IPipeline
    {
        object Execute(object param);

        IPipeline Register<TParam, TResult>(IPipelineFilter<TParam, TResult> filter);
    }

    public class Pipeline : IPipeline
    {
        private readonly ICollection<IPipelineFilter> _filters = new List<IPipelineFilter>();

        public object Execute(object param)
        {
            return _filters.Aggregate(param, (p, f) => f.Execute(p));
        }

        public IPipeline Register<TParam, TResult>(IPipelineFilter<TParam, TResult> filter)
        {
            _filters.Add(filter);
            return this;
        }
    }
}