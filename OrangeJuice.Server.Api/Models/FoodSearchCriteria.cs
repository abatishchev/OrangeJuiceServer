namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.FoodSearchCriteriaValidator))]
	public class FoodSearchCriteria
	{
		public string Title { get; set; }
	}
}