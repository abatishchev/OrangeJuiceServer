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
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class UserControllerTest
	{
		#region Ctor
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
		#endregion

		#region Get
		[TestMethod]
		public void Get_Should_Throw_Exception_When_UserGuid_Is_Empty()
		{
			//Arrange
			UserController controller = CreateController();
			Guid userGuid = Guid.Empty;

			// Act
			Action action = () => controller.Get(userGuid);

			// Assert
			action.ShouldThrow<ArgumentException>()
				  .And.ParamName.Should().Be("userGuid");
		}

		[TestMethod]
		public void Get_Should_Throw_Exception_When_User_By_Specified_UserGuid_Does_Not_Exist()
		{
			//Arrange
			Guid userGuid = Guid.NewGuid();
			var userRepository = CreateUserRepository();
			userRepository.Setup(r => r.Get(userGuid)).Returns<IUser>(null);

			UserController controller = CreateController(userRepository.Object);

			// Act
			Action action = () => controller.Get(userGuid);

			// Assert
			action.ShouldThrow<HttpResponseException>()
				  .And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public void Get_Should_Call_UserRepository_Get()
		{
			//Arrange
			bool called = false;
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Get(It.IsAny<Guid>()))
						  .Returns(new Mock<IUser>().Object)
						  .Callback(() =>
							  {
								  called = true;
							  });

			UserController controller = CreateController(userRepositoryMock.Object);

			// Act
			controller.Get(Guid.NewGuid());

			// Assert
			called.Should().BeTrue();
		}
		[TestMethod]
		public void Get_Should_ByPass_UserGuid_To_UserRepository_Get()
		{
			//Arrange
			Guid userGuid = Guid.NewGuid();
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Get(userGuid))
							  .Returns(new Mock<IUser>().Object);

			UserController controller = CreateController(userRepositoryMock.Object);

			// Act
			controller.Get(userGuid);

			// Assert
			userRepositoryMock.Verify(r => r.Get(userGuid), Times.Once());
		}
		#endregion

		#region Put
		[TestMethod]
		public void Put_Should_Return_BadRequest_When_UserRegistration_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();
			const UserRegistration userRegistration = null;
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpStatusCode actual = controller.Put(userRegistration).StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void Put_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController();
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (NewContext(CreateModelValidator(s => false)))
			{
				HttpStatusCode actual = controller.Put(userRegistration).StatusCode;

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
				HttpStatusCode actual = controller.Put(userRegistration).StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void Put_Should_Call_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			bool called = false;
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
							  .Callback<string>(e =>
								  {
									  called = true;
									  e.Should().Be(email);
								  });

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			using (NewContext(CreateModelValidator()))
			{
				controller.Put(userRegistration);

				// Assert
				called.Should().BeTrue();
			}
		}

		[TestMethod]
		public void Put_Should_ByPass_Email_To_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()));

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			using (NewContext(CreateModelValidator()))
			{
				controller.Put(userRegistration);

				// Assert
				userRepositoryMock.Verify(r => r.Register(email), Times.Once());
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
				HttpResponseMessage message = controller.Put(userRegistration);
				Guid actual = ((ObjectContent<Guid>)message.Content).GetValue();

				// Assert
				actual.Should().Be(expected);
			}
		}
		#endregion

		#region Helper methods
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
			IModelValidator current = ModelValidator.Current; ;
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