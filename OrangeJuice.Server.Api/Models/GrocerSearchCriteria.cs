using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	public class GrocerSearchCriteria
	{
		[Required]
		[StringLength(56, MinimumLength = 3)]
		public string Title { get; set; }
	}
}