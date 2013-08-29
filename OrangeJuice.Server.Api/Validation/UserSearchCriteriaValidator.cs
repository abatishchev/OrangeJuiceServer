﻿using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public class UserSearchCriteriaValidator : AbstractValidator<UserSearchCriteria>
	{
		public UserSearchCriteriaValidator()
		{
			RuleFor(x => x.UserGuid).NotNull();
		}
	}
}