namespace OrangeJuice.Server.Filters
{
	public interface IFilter<T>
	{
		T Filter(T item);
	}
}