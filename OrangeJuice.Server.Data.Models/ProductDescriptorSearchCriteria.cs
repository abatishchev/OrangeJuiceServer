namespace OrangeJuice.Server.Data.Models
{
	public class ProductDescriptorSearchCriteria
	{
		public string Operation { get; set; }

		public string SearchIndex { get; set; }

		public string[] ResponseGroups { get; set; }

		public string IdType { get; set; }

		public string ItemId { get; set; }
	}
}