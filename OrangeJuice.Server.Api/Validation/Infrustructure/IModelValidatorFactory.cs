using System.Collections.Generic;
using System.Web.Http.Validation;

using FluentValidation;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	public interface IModelValidatorFactory
	{
		ModelValidator Create(IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator);
	}
}