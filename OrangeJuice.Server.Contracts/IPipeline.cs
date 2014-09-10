using System;

namespace OrangeJuice.Server
{
	public interface IPipeline<T, TResult>
	{
		TResult Run(T input);
	}

}