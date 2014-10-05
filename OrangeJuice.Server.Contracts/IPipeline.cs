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
}