namespace OrangeJuice.Server.Filters
{
	public interface IFilter<in T>
	{
		bool Filter(T obj);
	}
}