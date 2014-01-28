namespace OrangeJuice.Server.Data
{
	public interface IRating
	{
		RatingId RatingId { get; }

		byte Value { get; set; }

		string Comment { get; set; }
	}
}