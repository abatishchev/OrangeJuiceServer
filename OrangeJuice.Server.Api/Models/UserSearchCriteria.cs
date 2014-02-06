using System;

using FluentValidation.Attributes;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(Validation.UserSearchCriteriaValidator))]
	public class UserSearchCriteria
	{
		public Guid UserId { get; set; }
	}
}