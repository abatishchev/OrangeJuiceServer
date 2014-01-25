namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.TitleSearchCriteriaValidator))]
	public class TitleSearchCriteria
	{
		public string Title { get; set; }
	}
}