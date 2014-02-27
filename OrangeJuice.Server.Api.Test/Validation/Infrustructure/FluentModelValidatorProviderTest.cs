using System;
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
		public void GetValidators_Should_Pass_IValidator_Of_MetaData_ContainerType_To_ValidatorFactory_GetValidator()
		{
			// Arrange
			IValidator validator = new Mock<IValidator>().Object;

			IValidatorFactory validatorFactory = CreateValidatorFactory(validator);
			IModelValidatorFactory modelValidatorFactory = CreateModelValidatorFactory();

			ModelValidatorProvider provider = CreateProvider(validatorFactory, modelValidatorFactory);

			ModelMetadata metadata = CreateMetadata(typeof(object));

			// Act
			provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>()).ToArray();

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

			// Act
			provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>()).ToArray();

			// Assert
			Mock.Get(modelValidatorFactory).Verify(f => f.Create(validatorProviders, validator), Times.Once);
		}

		[TestMethod]
		public void GetValidators_Should_Return_Empty_Sequence_When_ValidatorFactory_GetValidator_Returns_Null()
		{
			// Arrange
			IValidatorFactory validatorFactory = CreateValidatorFactory(null);
			ModelValidatorProvider provider = CreateProvider(validatorFactory);

			ModelMetadata metadata = CreateMetadata(typeof(object));

			// Act
			var result = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

			// Assert
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void GetValidators_Should_Return_Validator_Returned_By_ModelValidatorFactory_Create()
		{
			// Arrange
			ModelValidator modelValidator = CreateModelValidator();

			IModelValidatorFactory modelValidatorFactory = CreateModelValidatorFactory(modelValidator);
			IValidatorFactory validatorFactory = CreateValidatorFactory(Mock.Of<IValidator>());
			ModelValidatorProvider provider = CreateProvider(validatorFactory, modelValidatorFactory);

			ModelMetadata metadata = CreateMetadata(typeof(object));

			// Act
			var result = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

			// Assert
			result.Should().BeEquivalentTo(new[] { modelValidator });
		}
		#endregion

		#region Helper methods
		private static ModelValidatorProvider CreateProvider(IValidatorFactory validatorFactory = null, IModelValidatorFactory modelValidatorFactory = null)
		{
			return new FluentModelValidatorProvider(
				validatorFactory ?? new Mock<ValidatorFactoryBase>().Object,
				modelValidatorFactory ?? new Mock<IModelValidatorFactory>().Object);
		}

		private static ModelMetadata CreateMetadata(Type modelType)
		{
			return new ModelMetadata(new Mock<ModelMetadataProvider>().Object, modelType, () => null, typeof(object), "AnyPropertyName");
		}

		private static IValidatorFactory CreateValidatorFactory(IValidator validator)
		{
			var factoryMock = new Mock<IValidatorFactory>();
			factoryMock.Setup(f => f.GetValidator(It.IsAny<Type>())).Returns(validator);
			return factoryMock.Object;
		}

		private static IModelValidatorFactory CreateModelValidatorFactory(ModelValidator modelValidator = null)
		{
			var factoryMock = new Mock<IModelValidatorFactory>();
			factoryMock.Setup(f => f.Create(It.IsAny<IEnumerable<ModelValidatorProvider>>(), It.IsAny<IValidator>()))
					   .Returns(modelValidator ?? CreateModelValidator());
			return factoryMock.Object;
		}

		private static ModelValidator CreateModelValidator()
		{
			return new MockRepository(MockBehavior.Default).Create<ModelValidator>(Enumerable.Empty<ModelValidatorProvider>()).Object;
		}
		#endregion
	}
}