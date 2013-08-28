using System;
using System.Collections.Generic;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentValidation;

using HttpModelValidator = System.Web.Http.Validation.ModelValidator;

namespace OrangeJuice.Server.Api.Validation
{
	// TODO: tests
	internal sealed class FluentModelValidatorProvider : ModelValidatorProvider
	{
		private readonly ValidatorFactoryBase _validationFactory;

		public FluentModelValidatorProvider(ValidatorFactoryBase validationFactory)
		{
			if (validationFactory == null)
				throw new ArgumentNullException("validationFactory");

			_validationFactory = validationFactory;
		}

		public override IEnumerable<HttpModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
		{
			Type type = GetType(metadata);
			if (type != null)
			{
				IValidator validator = _validationFactory.CreateInstance(typeof(IValidator<>).MakeGenericType(type));
				yield return new FluentModelValidator(validatorProviders, validator);
			}
		}

		private static Type GetType(ModelMetadata metadata)
		{
			return metadata.ContainerType != null ? metadata.ContainerType.UnderlyingSystemType : null;
		}
	}
}