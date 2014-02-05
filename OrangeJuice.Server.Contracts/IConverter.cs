namespace OrangeJuice.Server
{
	public interface IConverter<TIn, TOut>
	{
		TOut Convert(TIn item);

		TIn ConvertBack(TOut item);
	}
}