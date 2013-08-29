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
		private readonly ValidatorFactoryBase _validatorFactory;
		private readonly IModelValidatorFactory _modelValidatorFactory;
		#endregion

		public FluentModelValidatorProvider(ValidatorFactoryBase validatorFactory, IModelValidatorFactory modelValidatorFactory)
		{
			if (validatorFactory == null)
				throw new ArgumentNullException("validatorFactory");
			if (modelValidatorFactory == null)
				throw new ArgumentNullException("modelValidatorFactory");

			_validatorFactory = validatorFactory;
			_modelValidatorFactory = modelValidatorFactory;
		}

		public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
		{
			if (metadata == null)
				throw new ArgumentNullException("metadata");
			if (validatorProviders == null)
				throw new ArgumentNullException("validatorProviders");

			Type type = GeType(metadata);
			IValidator validator = _validatorFactory.CreateInstance(typeof(IValidator<>).MakeGenericType(type));
			yield return _modelValidatorFactory.Create(validatorProviders, validator);
		}

		private static Type GeType(ModelMetadata metadata)
		{
			return metadata.ContainerType != null ? metadata.ContainerType.UnderlyingSystemType : null;
		}
	}
}