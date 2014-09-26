namespace OrangeJuice.Server
{
	// TODO: extract to a nuget package
	public interface IPipeline<in T, out TResult>
	{
		TResult Run(T input);
	}
}