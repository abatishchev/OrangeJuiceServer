namespace OrangeJuice.Server
{
	public interface IFactory<out T>
	{
		T Create();
	}

	public interface IFactory<in TIn, out TOut>
	{
		TOut Create(TIn input);
	}
}