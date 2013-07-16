using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Test.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void Put_Should_Return_BadRequest_When_Model_Not_IsValid()
        {
            // Arrange
            IModelValidator modelValidator = CreateModelValidator(s => false);
            UserController controller = CreateController(modelValidator);

            UserRegistration userRegistration = new UserRegistration();

            const HttpStatusCode expected = HttpStatusCode.BadRequest;

            // Act
            HttpStatusCode actual = controller.Post(userRegistration).StatusCode;

            // Assert
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void Put_Should_Return_Ok_When_Model_IsValid()
        {
            // Arrange
            IModelValidator modelValidator = CreateModelValidator(s => true);
            UserController controller = CreateController(modelValidator);

            UserRegistration userRegistration = new UserRegistration();

            const HttpStatusCode expected = HttpStatusCode.OK;

            // Act
            HttpStatusCode actual = controller.Post(userRegistration).StatusCode;

            // Assert
            actual.Should().Be(expected);
        }

        private static UserController CreateController(IModelValidator modelValidator)
        {
            UserController controller = new UserController(modelValidator);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties["MS_HttpConfiguration"] = new HttpConfiguration();
            return controller;
        }

        private static IModelValidator CreateModelValidator(Func<ModelStateDictionary, bool> isValidFunc)
        {
            var modelValidatorMock = new Mock<IModelValidator>();
            modelValidatorMock.Setup(v => v.IsValid(It.IsAny<ModelStateDictionary>())).Returns(isValidFunc);
            return modelValidatorMock.Object;
        }
    }
}