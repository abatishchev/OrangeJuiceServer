using System.Collections.Generic;
using System.Web.Http.Validation;

using FluentValidation;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	public sealed class FluentModelValidatorFactory : IModelValidatorFactory
	{
		public ModelValidator Create(IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator)
		{
			return new FluentModelValidator(validatorProviders, validator);
		}
	}
}