using System;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.UserSearchCriteriaValidator))]
	public class UserSearchCriteria
	{
		public Guid? UserGuid { get; set; }
	}
}