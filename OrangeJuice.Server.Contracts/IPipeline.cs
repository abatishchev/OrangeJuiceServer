using System;

namespace OrangeJuice.Server
{
	public interface IPipeline
	{
		object Execute(object param);
	}

	public interface IPipeline<T>
	{
		T Execute(T param);
	}

	public interface IPipeline<T, in U1>
	{
		T Execute(T param, U1 param1);
	}

	public interface IPipeline<T, in U1, in U2>
	{
		T Execute(T param, U1 param1, U2 param2);
	}
}