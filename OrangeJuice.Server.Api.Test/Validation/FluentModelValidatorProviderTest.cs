using System;

using FluentAssertions;

using FluentValidation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Test.Validation
{
	[TestClass]
	public class FluentModelValidatorProviderTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ValidationFactory_Is_Null()
		{
			// Arrange
			const ValidatorFactoryBase validationFactory = null;

			// Act
			Action action = () => new FluentModelValidatorProvider(validationFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("validationFactory");
		}

		[TestMethod]
		public void GetValidators_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}

		[TestMethod]
		public void GetType_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}
	}
}