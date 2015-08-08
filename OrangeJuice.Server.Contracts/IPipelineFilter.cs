namespace OrangeJuice.Server
{
	public interface IPipelineFilter
	{
	}

	public interface IPipelineFilter<T> : IPipelineFilter
	{
		T Execute(T param);
	}

	public interface IPipelineFilter<T, in U1> : IPipelineFilter
	{
		T Execute(T param, U1 param1);
	}

	public interface IPipelineFilter<T, in U1, in U2> : IPipelineFilter
	{
		T Execute(T param, U1 param1, U2 param2);
	}
}