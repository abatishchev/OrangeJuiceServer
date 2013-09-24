namespace OrangeJuice.Server
{
	public interface IValidator<in T>
	{
		bool IsValid(T item);
	}
}