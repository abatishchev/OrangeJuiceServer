using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentValidation;
using FluentValidation.Results;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	internal sealed class FluentModelValidator : ModelValidator
	{
		private readonly IValidator _validator;

		public FluentModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator)
			: base(validatorProviders)
		{
			if (validator == null)
				throw new ArgumentNullException("validator");
			_validator = validator;
		}

		public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object container)
		{
			if (metadata == null)
				throw new ArgumentNullException("metadata");
			if (container == null)
				throw new ArgumentNullException("container");

			// TODO: why is called twice?

			ValidationResult result = _validator.Validate(container);
			return from error in result.Errors
				   select new ModelValidationResult
				   {
					   Message = error.ErrorMessage,
					   MemberName = error.ErrorMessage
				   };
		}
	}
}