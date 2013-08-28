using System;
using System.Linq;
using System.Web.Http.Validation;

using FluentAssertions;

using FluentValidation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
		public void Validate_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}
	}
}