using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Test.Validation
{
	[TestClass]
	public class FluentModelValidatorTest
	{
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

		[TestMethod]
		public void Validate_Should_Return_Empty_Sequence_When_Underlying_Validator_Validate_Returned_No_Errors()
		{
			// Arrange
			IEnumerable<ModelValidatorProvider> validatorProviders = Enumerable.Empty<ModelValidatorProvider>();
			var underlyingValidator = new Mock<IValidator>();
			underlyingValidator.Setup(v => v.Validate(It.IsAny<object>())).Returns(new ValidationResult(Enumerable.Empty<ValidationFailure>()));

			ModelValidator validator = new FluentModelValidator(validatorProviders, underlyingValidator.Object);

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

			IEnumerable<ModelValidatorProvider> validatorProviders = Enumerable.Empty<ModelValidatorProvider>();
			var underlyingValidator = new Mock<IValidator>();
			underlyingValidator.Setup(v => v.Validate(It.IsAny<object>()))
							   .Returns(new ValidationResult(Enumerable.Repeat(CreateValidationFailure(), count)));

			ModelValidator validator = new FluentModelValidator(validatorProviders, underlyingValidator.Object);

			ModelMetadata metadata = CreateMetadata();
			object container = new object();

			// Act
			ModelValidationResult[] result = validator.Validate(metadata, container).ToArray();

			// Assert
			result.Should().HaveCount(count);
		}

		#region Helper methods
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