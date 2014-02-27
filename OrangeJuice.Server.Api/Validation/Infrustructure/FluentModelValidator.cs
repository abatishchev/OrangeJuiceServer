using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentValidation;
using FluentValidation.Results;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	public sealed class FluentModelValidator : ModelValidator
	{
		private readonly IValidator _validator;

		public FluentModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, IValidator validator)
			: base(validatorProviders)
		{
			_validator = validator;
		}

		public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object container)
		{
			if (metadata.Model == null)
				return Enumerable.Empty<ModelValidationResult>();

			ValidationResult result = _validator.Validate(container);
			if (result.IsValid)
				return Enumerable.Empty<ModelValidationResult>();

			return from error in result.Errors
				   select new ModelValidationResult
				   {
					   Message = error.ErrorMessage,
					   MemberName = error.PropertyName
				   };

		}
	}
}