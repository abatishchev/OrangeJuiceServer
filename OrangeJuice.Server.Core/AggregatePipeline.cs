using System;
using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public abstract class AggregatePipeline<T> : IPipeline<T>
	{

		public abstract IEnumerable<Func<T, T>> GetOperations();

		public T Run(T input)
		{
			return GetOperations().Aggregate(input, (param, op) => op(param));
		}
	}
}
