using System;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Validation.Infrustructure;

namespace OrangeJuice.Server.Api.Test.Validation.Infrustructure
{
	[TestClass]
	public class FluentModelValidatorTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Validator_Is_Null()
		{
			// Arrange
			var validatorProviders = Enumerable.Empty<ModelValidatorProvider>();
			const IValidator validator = null;

			// Act
			Action action = () => new FluentModelValidator(validatorProviders, validator);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
			      .And.ParamName.Should().Be("validator");
		}
		#endregion

		#region Validate
		[TestMethod]
		public void Validate_Should_Throw_Exception_When_Metadata_Is_Null()
		{
			// Arrange
			const ModelMetadata metadata = null;
			const object container = null;

			ModelValidator validator = CreateModelValidator();

			// Act
			Action action = () => validator.Validate(metadata, container);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
			      .And.ParamName.Should().Be("metadata");
		}

		[TestMethod]
		public void Validate_Should_Throw_Exception_When_Container_Is_Null()
		{
			// Arrange
			ModelMetadata metadata = CreateMetadata();
			const object container = null;

			ModelValidator validator = CreateModelValidator();

			// Act
			Action action = () => validator.Validate(metadata, container);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
			      .And.ParamName.Should().Be("container");
		}

		[TestMethod]
		public void Validate_Should_Return_Empty_Sequence_When_Underlying_Validator_Validate_Returned_No_Errors()
		{
			// Arrange
			var underlyingValidator = new Mock<IValidator>();
			underlyingValidator.Setup(v => v.Validate(It.IsAny<object>())).Returns(new ValidationResult(Enumerable.Empty<ValidationFailure>()));

			ModelValidator validator = CreateModelValidator(underlyingValidator.Object);

			ModelMetadata metadata = CreateMetadata();
			object container = new object();

			// Act
			ModelValidationResult[] result = validator.Validate(metadata, container).ToArray();

			// Assert
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void Validate_Should_Return_ModelValidationResult_For_Each_ValidationFailure_Returned_By_Underlying_Validator_Validate()
		{
			// Arrange
			const int count = 3;

			var underlyingValidator = new Mock<IValidator>();
			underlyingValidator.Setup(v => v.Validate(It.IsAny<object>()))
			                   .Returns(new ValidationResult(Enumerable.Repeat(CreateValidationFailure(), count)));

			ModelValidator validator = CreateModelValidator(underlyingValidator.Object);

			ModelMetadata metadata = CreateMetadata();
			object container = new object();

			// Act
			ModelValidationResult[] result = validator.Validate(metadata, container).ToArray();

			// Assert
			result.Should().HaveCount(count);
		}
		#endregion

		#region Helper methods
		private static FluentModelValidator CreateModelValidator(IValidator validator = null)
		{
			return new FluentModelValidator(Enumerable.Empty<ModelValidatorProvider>(), validator ?? new Mock<IValidator>().Object);
		}

		private static ModelMetadata CreateMetadata(Type type = null)
		{
			return new ModelMetadata(new Mock<ModelMetadataProvider>().Object, type ?? typeof(object), () => new object(), type ?? typeof(object), "AnyPropertyName");
		}

		private static ValidationFailure CreateValidationFailure()
		{
			return new ValidationFailure("anyProperty", "anyError");
		}
		#endregion
	}
}