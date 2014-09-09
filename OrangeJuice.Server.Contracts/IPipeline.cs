using System;

namespace OrangeJuice.Server
{
	public interface IPipeline<T>
	{
		// System.Collections.Generic.IEnumerable<Func<T, T>> GetOperations();
		T Run(T input);
	}

}