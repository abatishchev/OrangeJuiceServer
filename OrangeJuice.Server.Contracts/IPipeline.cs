namespace OrangeJuice.Server
{
	public interface IPipeline
	{
		object Execute(object param);

		IPipeline Register<TParam, TResult>(IPipelineFilter<TParam, TResult> filter);
	}
}