namespace OrangeJuice.Server.Api.Models
{
	public class GroceryDescription
	{
		// ReSharper disable once InconsistentNaming
		public string ASIN { get; set; }

		public string Title { get; set; }

		public string Manufacturer { get; set; }

		public string DetailPageUrl { get; set; }

		public string TechnicalDetailsUrl { get; set; }
	}
}