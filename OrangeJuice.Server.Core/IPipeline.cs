using System;
using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server
{
	public interface IPipeline<T>
	{
		IEnumerable<Func<T, T>> GetOperations();
	}

	public static class PipelineExtensions
	{
		public static T Run<T>(this IPipeline<T> pipeline, T input)
		{
			return pipeline.GetOperations().Aggregate(input, (param, op) => op(param));
		}
	}
}