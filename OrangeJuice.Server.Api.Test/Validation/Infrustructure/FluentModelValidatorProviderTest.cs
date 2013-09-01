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
	[TestClass]
	public class FluentModelValidatorProviderTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ValidatorFactory_Is_Null()
		{
			// Arrange
			const ValidatorFactoryBase validatorFactory = null;
			const IModelValidatorFactory modelValidatorFactory = null;

			// Act
			Action action = () => new FluentModelValidatorProvider(validatorFactory, modelValidatorFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("validatorFactory");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ModelalidatorFactory_Is_Null()
		{
			// Arrange
			ValidatorFactoryBase validatorFactory = new Mock<ValidatorFactoryBase>().Object;
			const IModelValidatorFactory modelValidatorFactory = null;

			// Act
			Action action = () => new FluentModelValidatorProvider(validatorFactory, modelValidatorFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("modelValidatorFactory");
		}
		#endregion

		#region GetValidators
		[TestMethod]
		public void GetValidators_Should_Throw_Exception_When_Metadata_Is_Null()
		{
			// Arrange
			ModelValidatorProvider provider = CreateProvider();

			const ModelMetadata metadata = null;
			const IEnumerable<ModelValidatorProvider> validatorProviders = null;

			// Act
			Action action = () => provider.GetValidators(metadata, validatorProviders).ToArray();

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("metadata");
		}

		[TestMethod]
		public void GetValidators_Should_Throw_Exception_When_ValidatorProviders_Is_Null()
		{
			// Arrange
			ModelValidatorProvider provider = CreateProvider();

			ModelMetadata metadata = CreateMetadata();
			const IEnumerable<ModelValidatorProvider> validatorProviders = null;

			// Act
			Action action = () => provider.GetValidators(metadata, validatorProviders).ToArray();

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("validatorProviders");
		}

		[TestMethod]
		public void GetValidators_Should_Pass_IValidator_Of_MetaData_ContainerType_UnderlyingSystemType_To_ValidatorFactory_CreateInstance()
		{
			// Arrange
			IValidator validator = new Mock<IValidator>().Object;

			var validatorFactoryMock = CreateValidatorFactory(validator);
			var modelValidatorFactoryMock = CreateModelValidatorFactory();

			ModelValidatorProvider provider = CreateProvider(validatorFactoryMock.Object, modelValidatorFactoryMock.Object);

			ModelMetadata metadata = CreateMetadata(typeof(object));
			IEnumerable<ModelValidatorProvider> modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			provider.GetValidators(metadata, modelValidatorProviders).ToArray();

			// Assert
			validatorFactoryMock.Verify(f => f.CreateInstance(It.Is<Type>(t => t.IsAssignableFrom(typeof(IValidator<object>)))), Times.Once());
		}

		[TestMethod]
		public void GetValidators_Should_Pass_ValidatorProviders_And_Validator_To_ModelValidatorFactory_Create()
		{
			// Arrange
			IEnumerable<ModelValidatorProvider> validatorProviders = Enumerable.Empty<ModelValidatorProvider>();
			IValidator validator = new Mock<IValidator>().Object;

			var validatorFactoryMock = CreateValidatorFactory(validator);
			var modelValidatorFactoryMock = CreateModelValidatorFactory();

			ModelValidatorProvider provider = CreateProvider(validatorFactoryMock.Object, modelValidatorFactoryMock.Object);

			ModelMetadata metadata = CreateMetadata(typeof(object));
			IEnumerable<ModelValidatorProvider> modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			provider.GetValidators(metadata, modelValidatorProviders).ToArray();

			// Assert
			modelValidatorFactoryMock.Verify(f => f.Create(validatorProviders, validator), Times.Once());
		}

		[TestMethod]
		public void GetValidators_Should_Return_Empty_Sequence_When_MetaData_ContainerType_UnderlyingSystemType_Is_Null()
		{
			// Arrange
			ModelValidatorProvider provider = CreateProvider();

			ModelMetadata metadata = CreateMetadata(contrainerType: null);
			IEnumerable<ModelValidatorProvider> modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();

			// Act
			var result = provider.GetValidators(metadata, modelValidatorProviders);

			// Assert
			result.Should().BeEmpty();
		}
		#endregion

		#region Helper methods
		private static ModelValidatorProvider CreateProvider(ValidatorFactoryBase validatorFactory = null, IModelValidatorFactory modelValidatorFactory = null)
		{
			return new FluentModelValidatorProvider(
				validatorFactory ?? new Mock<ValidatorFactoryBase>().Object,
				modelValidatorFactory ?? new Mock<IModelValidatorFactory>().Object);
		}

		private static ModelMetadata CreateMetadata(Type contrainerType = null)
		{
			return new ModelMetadata(new Mock<ModelMetadataProvider>().Object, contrainerType, () => null, typeof(object), "AnyPropertyName");
		}

		private static Mock<ValidatorFactoryBase> CreateValidatorFactory(IValidator validator)
		{
			var factoryMock = new Mock<ValidatorFactoryBase>();
			factoryMock.Setup(f => f.CreateInstance(It.IsAny<Type>())).Returns(validator);
			return factoryMock;
		}

		private static Mock<IModelValidatorFactory> CreateModelValidatorFactory()
		{
			var factoryMock = new Mock<IModelValidatorFactory>(MockBehavior.Strict);
			factoryMock.Setup(f => f.Create(It.IsAny<IEnumerable<ModelValidatorProvider>>(), It.IsAny<IValidator>()))
					   .Returns<IEnumerable<ModelValidatorProvider>, IValidator>((p, v) => new FluentModelValidator(p, v));
			return factoryMock;
		}
		#endregion
	}
}