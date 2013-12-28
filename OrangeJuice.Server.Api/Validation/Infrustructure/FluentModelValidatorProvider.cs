using System;
using System.Collections.Generic;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentValidation;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	public sealed class FluentModelValidatorProvider : ModelValidatorProvider
	{
		#region Fields
		private readonly IValidatorFactory _validatorFactory;
		private readonly IModelValidatorFactory _modelValidatorFactory;
		#endregion

		#region Ctor
		public FluentModelValidatorProvider(IValidatorFactory validatorFactory, IModelValidatorFactory modelValidatorFactory)
		{
			_validatorFactory = validatorFactory;
			_modelValidatorFactory = modelValidatorFactory;
		}
		#endregion

		#region Methods
		public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
		{
			Type type = GeType(metadata);
			if (type != null)
			{
				IValidator validator = _validatorFactory.GetValidator(type);
				yield return _modelValidatorFactory.Create(validatorProviders, validator);
			}
		}

		private static Type GeType(ModelMetadata metadata)
		{
			return metadata.ContainerType != null ? metadata.ContainerType.UnderlyingSystemType : null;
		}
		#endregion
	}
}