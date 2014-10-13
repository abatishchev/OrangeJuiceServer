namespace OrangeJuice.Server
{
	public interface IConverter<TIn, TOut>
	{
		TOut Convert(TIn value);

		TIn ConvertBack(TOut value);
	}
}