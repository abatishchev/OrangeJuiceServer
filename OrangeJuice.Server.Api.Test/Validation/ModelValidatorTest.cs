using System;
using System.Web.Http.ModelBinding;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Test.Validation
{
	[TestClass]
	public class ModelValidatorTest
	{
		#region Test methods
		[TestMethod]
		public void IsValid_Should_Return_True_When_Model_IsValid()
		{
			// Arrange
			ModelStateDictionary modelState = new ModelStateDictionary();

			// Act
			bool isValid = modelState.IsValid;

			// Arrange
			isValid.Should().BeTrue();
		}

		[TestMethod]
		public void IsValid_Should_Return_False_When_Model_Not_IsValid()
		{
			// Arrange
			ModelStateDictionary modelState = new ModelStateDictionary();
			modelState.AddModelError("Key", "ErrorMessage");

			// Act
			bool isValid = modelState.IsValid;

			// Arrange
			isValid.Should().BeFalse();
		}

		[TestMethod]
		public void Current_Should_Not_Be_Null()
		{
			ModelValidator.Current.Should().NotBeNull();
		}

		[TestMethod]
		public void Current_Should_Be_Of_Type_ModelValidator_By_Default()
		{
			using (NewContext())
			{
				ModelValidator.Current.Should().BeOfType<ModelValidator>();
			}
		}

		[TestMethod]
		public void Current_Should_Return_Validator_Was_Previously_Explictly_Set()
		{
			using (NewContext())
			{
				// Arrange
				IModelValidator modelValidator = new Mock<IModelValidator>().Object;

				// Act
				ModelValidator.Current = modelValidator;

				// Aassert
				ModelValidator.Current.Should().Be(modelValidator);
			}
		}
		#endregion

		#region Helper methods
		private static IDisposable NewContext(IModelValidator modelValidator = null)
		{
			IModelValidator current = ModelValidator.Current;
			return new TestContext(() =>
			{
				ModelValidator.Current = modelValidator;
			}, () =>
			{
				ModelValidator.Current = current;
			});
		}
		#endregion
	}
}