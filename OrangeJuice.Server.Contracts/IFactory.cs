namespace OrangeJuice.Server
{
	public interface IFactory<out T>
	{
		T Create();
	}
}