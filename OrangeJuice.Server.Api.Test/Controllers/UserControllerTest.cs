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
            using (NewContext(CreateModelValidator(s => false)))
            {
                UserController controller = CreateController();
                UserRegistration userRegistration = new UserRegistration();
                const HttpStatusCode expected = HttpStatusCode.BadRequest;

                // Act
                HttpStatusCode actual = controller.Post(userRegistration).StatusCode;

                // Assert
                actual.Should().Be(expected);
            }
        }

        [TestMethod]
        public void Put_Should_Return_Ok_When_Model_IsValid()
        {
            // Arrange
            UserController controller = CreateController();
            UserRegistration userRegistration = new UserRegistration();
            const HttpStatusCode expected = HttpStatusCode.OK;

            // Act
            using (NewContext(CreateModelValidator()))
            {

                HttpStatusCode actual = controller.Post(userRegistration).StatusCode;

                // Assert
                actual.Should().Be(expected);
            }
        }

        [TestMethod]
        public void Put_Should_Call_UserRepository_Register()
        {
            // Arrange
            const string expected = "test@example.com";
            bool called = false;
            var userRepositoryMock = CreateUserRepository();
            userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
                              .Callback<string>(e =>
                                  {
                                      called = true;
                                      e.Should().Be(expected);
                                  });

            UserController controller = CreateController(userRepositoryMock.Object);
            UserRegistration userRegistration = new UserRegistration
            {
                Email = expected
            };

            // Act
            using (NewContext(CreateModelValidator()))
            {
                controller.Post(userRegistration);

                // Assert
                called.Should().BeTrue();
            }
        }

        [TestMethod]
        public void Put_Should_Return_Guid_Of_Created_User()
        {
            // Arrange
            Guid expected = Guid.NewGuid();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
                           .Returns(expected);

            UserController controller = CreateController(userRepositoryMock.Object);
            UserRegistration userRegistration = new UserRegistration();

            // Act
            using (NewContext(CreateModelValidator(s => true)))
            {
                Guid actual = ((ObjectContent<Guid>)controller.Post(userRegistration).Content).GetValue();

                // Assert
                actual.Should().Be(expected);
            }
        }

        private static UserController CreateController(IUserRepository userRepository = null)
        {
            return ControllerFactory.Create<UserController>(userRepository ?? CreateUserRepository().Object);
        }

        private static IModelValidator CreateModelValidator(Func<ModelStateDictionary, bool> isValidFunc = null)
        {
            var modelValidatorMock = new Mock<IModelValidator>();
            modelValidatorMock.Setup(v => v.IsValid(It.IsAny<ModelStateDictionary>())).Returns(isValidFunc ?? (s => true));
            return modelValidatorMock.Object;
        }

        private static Mock<IUserRepository> CreateUserRepository()
        {
            return new Mock<IUserRepository>();
        }

        private static IDisposable NewContext(IModelValidator modelValidator)
        {
            IModelValidator current = ModelValidator.Current;;
            return new TestContext(() =>
                {
                    ModelValidator.Current = modelValidator;
                }, () =>
                {
                    ModelValidator.Current = current;
                });
        }
    }
}