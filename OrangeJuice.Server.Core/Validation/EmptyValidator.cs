namespace OrangeJuice.Server.Validation
{
	public sealed class EmptyValidator<T> : IValidator<T, string>
	{
		public bool IsValid(T item)
		{
			return true;
		}

		public string ValidationResult { get; set; }
	}
}