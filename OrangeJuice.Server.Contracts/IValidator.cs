namespace OrangeJuice.Server
{
	public interface IValidator<in T>
	{
		bool IsValid(T item);
	}

	public interface IValidator<in T, out TResult> : IValidator<T>
	{
		TResult ValidationResult { get; }
	}
}