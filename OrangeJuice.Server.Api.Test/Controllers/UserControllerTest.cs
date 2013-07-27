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

		#region GetUserInformation
		[TestMethod]
		public void GetUser_Should_Return_BadRequest_When_UserInformation_Is_Null()
		{
			// Arrange
			const UserInformation userInformation = null;
			UserController controller = CreateController();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpStatusCode actual = controller.GetUserInformation(userInformation).StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void GetUser_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController();
			UserInformation userInformation = new UserInformation();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (NewContext(CreateModelValidator(s => false)))
			{
				HttpStatusCode actual = controller.GetUserInformation(userInformation).StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void GetUser_Should_Throw_Exception_When_User_By_Specified_UserGuid_Does_Not_Exist()
		{
			//Arrange
			Guid userGuid = Guid.NewGuid();
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Find(userGuid))
							  .Returns<IUser>(null);

			UserController controller = CreateController(userRepositoryMock);
			UserInformation userInformation = new UserInformation
			{
				UserKey = userGuid
			};

			// Act
			Action action = () => controller.GetUserInformation(userInformation);

			// Assert
			action.ShouldThrow<HttpResponseException>()
				  .And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public void GetUser_Should_Call_UserRepository_Find()
		{
			//Arrange
			bool called = false;
			IUser user = CreateUser();
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Find(It.IsAny<Guid>()))
						  .Returns(user)
						  .Callback(() =>
							  {
								  called = true;
							  });

			UserController controller = CreateController(userRepositoryMock);
			UserInformation userInformation = new UserInformation
			{
				//UserKey = Guid.NewGuid()
			};

			// Act
			controller.GetUserInformation(userInformation);

			// Assert
			called.Should().BeTrue();
		}

		[TestMethod]
		public void GetUser_Should_ByPass_UserGuid_To_UserRepository_Find()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser user = CreateUser(userGuid);

			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Find(userGuid))
							  .Returns(user);

			UserController controller = CreateController(userRepositoryMock);
			UserInformation userInformation = new UserInformation
			{
				UserKey = userGuid
			};

			// Act
			controller.GetUserInformation(userInformation);

			// Assert
			userRepositoryMock.Verify(r => r.Find(userGuid), Times.Once());
		}

		[TestMethod]
		public void GetUser_Should_Return_User_By_Specified_UserGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser expected = CreateUser(userGuid);

			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Find(userGuid))
							  .Returns(expected);

			UserController controller = CreateController(userRepositoryMock);
			UserInformation userInformation = new UserInformation
			{
				UserKey = userGuid
			};

			// Act
			HttpResponseMessage message = controller.GetUserInformation(userInformation);

			// Assert
			IUser actual = message.Content.GetValue<IUser>();
			actual.Should().Be(expected);
		}
		#endregion

		#region PutUserRegistration
		[TestMethod]
		public void PutUser_Should_Return_BadRequest_When_UserRegistration_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();
			const UserRegistration userRegistration = null;
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpStatusCode actual = controller.PutUserRegistration(userRegistration).StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void PutUser_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController();
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (NewContext(CreateModelValidator(s => false)))
			{
				HttpStatusCode actual = controller.PutUserRegistration(userRegistration).StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void PutUser_Should_Return_Ok_When_Model_IsValid()
		{
			// Arrange
			IUser user = CreateUser();
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
							  .Returns(user);

			UserController controller = CreateController(userRepositoryMock);
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.OK;

			// Act
			using (NewContext(CreateModelValidator()))
			{
				HttpStatusCode actual = controller.PutUserRegistration(userRegistration).StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void PutUser_Should_Return_InternalError_When_User_Repository_Register_Returns_Null()
		{
			// Arrange
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
							  .Returns<IUser>(null);

			UserController controller = CreateController(userRepositoryMock);
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.InternalServerError;

			// Act
			using (NewContext(CreateModelValidator()))
			{
				HttpStatusCode actual = controller.PutUserRegistration(userRegistration).StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void PutUser_Should_Call_UserRepository_Register()
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

			UserController controller = CreateController(userRepositoryMock);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			using (NewContext(CreateModelValidator()))
			{
				controller.PutUserRegistration(userRegistration);

				// Assert
				called.Should().BeTrue();
			}
		}

		[TestMethod]
		public void PutUser_Should_ByPass_Email_To_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			var userRepositoryMock = CreateUserRepository();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()));

			UserController controller = CreateController(userRepositoryMock);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			using (NewContext(CreateModelValidator()))
			{
				controller.PutUserRegistration(userRegistration);

				// Assert
				userRepositoryMock.Verify(r => r.Register(email), Times.Once());
			}
		}

		[TestMethod]
		public void PutUser_Should_Return_Guid_Of_Created_User()
		{
			// Arrange
			Guid expected = Guid.NewGuid();
			IUser user = CreateUser(expected);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
							  .Returns(user);

			UserController controller = CreateController(userRepositoryMock);
			UserRegistration userRegistration = new UserRegistration();

			// Act
			using (NewContext(CreateModelValidator(s => true)))
			{
				HttpResponseMessage message = controller.PutUserRegistration(userRegistration);
				Guid actual = message.Content.GetValue<Guid>();

				// Assert
				actual.Should().Be(expected);
			}
		}
		#endregion

		#region Helper methods
		private static UserController CreateController(Mock<IUserRepository> userRepositoryMock = null)
		{
			return ControllerFactory.Create<UserController>((userRepositoryMock ?? CreateUserRepository()).Object);
		}

		private static IModelValidator CreateModelValidator(Func<ModelStateDictionary, bool> isValidFunc = null)
		{
			var modelValidatorMock = new Mock<IModelValidator>();
			modelValidatorMock.Setup(v => v.IsValid(It.IsAny<ModelStateDictionary>())).Returns(isValidFunc ?? (s => true));
			return modelValidatorMock.Object;
		}

		private static IUser CreateUser(Guid? userGuid = null)
		{
			var userMock = new Mock<IUser>();
			userMock.Setup(u => u.UserGuid)
					.Returns(userGuid ?? Guid.NewGuid());
			return userMock.Object;
		}

		// TODO: simplify moq setup
		private static Mock<IUserRepository> CreateUserRepository()
		{
			return new Mock<IUserRepository>();
		}

		private static IDisposable NewContext(IModelValidator modelValidator)
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