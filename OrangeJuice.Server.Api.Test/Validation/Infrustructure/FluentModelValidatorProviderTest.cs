﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentAssertions;

using FluentValidation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Validation.Infrustructure;

namespace OrangeJuice.Server.Api.Test.Validation.Infrustructure
{
	// ReSharper disable ReturnValueOfPureMethodIsNotUsed
	[TestClass]
	public class FluentModelValidatorProviderTest
	{
		#region GetValidators
		[TestMethod]
		public void GetValidators_Should_Pass_IValidator_Of_MetaData_ContainerType_UnderlyingSystemType_To_ValidatorFactory_GetValidator()
		{
			// Arrange
			IValidator validator = new Mock<IValidator>().Object;

			IValidatorFactory validatorFactory = CreateValidatorFactory(validator);
			IModelValidatorFactory modelValidatorFactory = CreateModelValidatorFactory();

			ModelValidatorProvider provider = CreateProvider(validatorFactory, modelValidatorFactory);

			ModelMetadata metadata = CreateMetadata(typeof(object));
			var modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			provider.GetValidators(metadata, modelValidatorProviders).ToArray();

			// Assert
			Mock.Get(validatorFactory).Verify(f => f.GetValidator(It.Is<Type>(t => t.IsAssignableFrom(typeof(IValidator<object>)))), Times.Once);
		}

		[TestMethod]
		public void GetValidators_Should_Pass_ValidatorProviders_And_Validator_To_ModelValidatorFactory_Create()
		{
			// Arrange
			IEnumerable<ModelValidatorProvider> validatorProviders = Enumerable.Empty<ModelValidatorProvider>();
			IValidator validator = new Mock<IValidator>().Object;

			IValidatorFactory validatorFactory = CreateValidatorFactory(validator);
			IModelValidatorFactory modelValidatorFactory = CreateModelValidatorFactory();

			ModelValidatorProvider provider = CreateProvider(validatorFactory, modelValidatorFactory);

			ModelMetadata metadata = CreateMetadata(typeof(object));
			var modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			provider.GetValidators(metadata, modelValidatorProviders).ToArray();

			// Assert
			Mock.Get(modelValidatorFactory).Verify(f => f.Create(validatorProviders, validator), Times.Once);
		}

		[TestMethod]
		public void GetValidators_Should_Return_Empty_Sequence_When_MetaData_ContainerType_UnderlyingSystemType_Is_Null()
		{
			// Arrange
			ModelValidatorProvider provider = CreateProvider();

			ModelMetadata metadata = CreateMetadata(contrainerType: null);
			var modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			var result = provider.GetValidators(metadata, modelValidatorProviders);

			// Assert
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void GetValidators_Should_Return_Empty_Sequece_When_ValidatorFactory_GetValidator_Returns_Null()
		{
			// Arrange
			IValidatorFactory validatorFactory = CreateValidatorFactory(null);
			ModelValidatorProvider provider = CreateProvider(validatorFactory);

			ModelMetadata metadata = CreateMetadata(typeof(object));
			var modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			var result = provider.GetValidators(metadata, modelValidatorProviders);

			// Assert
			result.Should().BeEmpty();
		}
		#endregion

		#region Helper methods
		private static ModelValidatorProvider CreateProvider(IValidatorFactory validatorFactory = null, IModelValidatorFactory modelValidatorFactory = null)
		{
			return new FluentModelValidatorProvider(
				validatorFactory ?? new Mock<ValidatorFactoryBase>().Object,
				modelValidatorFactory ?? new Mock<IModelValidatorFactory>().Object);
		}

		private static ModelMetadata CreateMetadata(Type contrainerType)
		{
			return new ModelMetadata(new Mock<ModelMetadataProvider>().Object, contrainerType, () => null, typeof(object), "AnyPropertyName");
		}

		private static IValidatorFactory CreateValidatorFactory(IValidator validator)
		{
			var factoryMock = new Mock<IValidatorFactory>();
			factoryMock.Setup(f => f.GetValidator(It.IsAny<Type>())).Returns(validator);
			return factoryMock.Object;
		}

		private static IModelValidatorFactory CreateModelValidatorFactory()
		{
			var factoryMock = new Mock<IModelValidatorFactory>();
			factoryMock.Setup(f => f.Create(It.IsAny<IEnumerable<ModelValidatorProvider>>(), It.IsAny<IValidator>()))
					   .Returns<IEnumerable<ModelValidatorProvider>, IValidator>((p, v) => new FluentModelValidator(p, v));
			return factoryMock.Object;
		}
		#endregion
	}
}