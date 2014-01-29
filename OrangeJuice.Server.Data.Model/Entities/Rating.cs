// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Data
{
	public partial class Rating : IRating
	{
		public RatingId RatingId
		{
			get
			{
				return new RatingId
				{
					UserId = UserId,
					ProductId = ProductId
				};
			}
			set
			{
				UserId = value.UserId;
				ProductId = value.ProductId;
			}
		}
	}
}