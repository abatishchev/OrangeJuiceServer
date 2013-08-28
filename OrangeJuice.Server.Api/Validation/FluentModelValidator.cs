using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentValidation;
using FluentValidation.Results;

namespace OrangeJuice.Server.Api.Validation
{
	// TODO: tests
	internal sealed class FluentModelValidator : System.Web.Http.Validation.ModelValidator
	{
		private readonly IValidator _validator;

		public FluentModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator)
			: base(validatorProviders)
		{
			if (validator == null)
				throw new ArgumentNullException("validator");
			_validator = validator;
		}

		public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object instance)
		{
			ValidationResult result = _validator.Validate(instance);
			return from error in result.Errors
				   select new ModelValidationResult
				   {
					   Message = error.ErrorMessage,
					   MemberName = error.ErrorMessage
				   };
		}
	}
}