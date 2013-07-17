using System.Web.Http.ModelBinding;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Test.Validation
{
    [TestClass]
    public class ModelValidatorTest
    {
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
            // Arrange
            // Act
            IModelValidator modelValidator = ModelValidator.Current;

            // Arrange
            modelValidator.Should().NotBeNull();
        }

        [TestMethod]
        public void Current_Should_Be_Of_Type_ModelValidator()
        {
            // Arrange
            // Act
            IModelValidator modelValidator = ModelValidator.Current;

            // Arrange
            modelValidator.Should().BeOfType<ModelValidator>();
        }
    }
}