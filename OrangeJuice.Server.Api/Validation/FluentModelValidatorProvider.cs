using System;
using System.Collections.Generic;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentValidation;

namespace OrangeJuice.Server.Api.Validation
{
	internal sealed class FluentModelValidatorProvider : ModelValidatorProvider
	{
		#region Fields
		private readonly ValidatorFactoryBase _validationFactory;
		#endregion

		public FluentModelValidatorProvider(ValidatorFactoryBase validationFactory)
		{
			if (validationFactory == null)
				throw new ArgumentNullException("validationFactory");

			_validationFactory = validationFactory;
		}

		public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
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