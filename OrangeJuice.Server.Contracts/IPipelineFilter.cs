namespace OrangeJuice.Server
{
	public interface IPipelineFilter
	{
		object Execute(object param);
	}

	public interface IPipelineFilter<in TParam, out TResult> : IPipelineFilter
	{
		TResult Execute(TParam param);
	}
}