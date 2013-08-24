
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	public class FoodSearchCriteria
	{
		[Required]
		[StringLength(56, MinimumLength = 3)]
		public string Title { get; set; }
	}
}