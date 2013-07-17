using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ModelBinding;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void Ctor_Should_Throw_Exception_When_UserRepository_Is_Null()
        {
            //Arrange
            const IUserRepository userRepository = null;

            // Act
            Action action = () => new UserController(userRepository);

            // Assert
            action.ShouldThrow<ArgumentException>()
                  .And.ParamName.Should().Be("userRepository");
        }

        [TestMethod]
        public void Put_Should_Return_BadRequest_When_Model_Not_IsValid()
        {
            // Arrange
            ModelValidator.Current = CreateModelValidator(s => false);
            UserController controller = CreateController();

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
            ModelValidator.Current = CreateModelValidator(s => true);
            UserController controller = CreateController();

            UserRegistration userRegistration = new UserRegistration();

            const HttpStatusCode expected = HttpStatusCode.OK;

            // Act
            HttpStatusCode actual = controller.Post(userRegistration).StatusCode;

            // Assert
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void Put_Should_Return_Guid_Of_Created_User()
        {
            // Arrange
            Guid expected = Guid.NewGuid();
            IUserRepository userRepository = CreateUserRepository(expected);
            ModelValidator.Current = CreateModelValidator(s => true);

            UserController controller = CreateController(userRepository);
            UserRegistration userRegistration = new UserRegistration();

            // Act
            Guid actual = ((ObjectContent<Guid>)controller.Post(userRegistration).Content).GetValue();

            // Assert
            actual.Should().Be(expected);
        }

        private static UserController CreateController(IUserRepository userRepository = null)
        {
            UserController controller = ControllerFactory.Create<UserController>(userRepository ?? CreateUserRepository());
            return controller;
        }

        private static IModelValidator CreateModelValidator(Func<ModelStateDictionary, bool> isValidFunc)
        {
            var modelValidatorMock = new Mock<IModelValidator>();
            modelValidatorMock.Setup(v => v.IsValid(It.IsAny<ModelStateDictionary>())).Returns(isValidFunc);
            return modelValidatorMock.Object;
        }

        private static IUserRepository CreateUserRepository(Guid? userId = null)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
                              .Returns(userId ?? Guid.NewGuid());
            return userRepositoryMock.Object;
        }
    }
}