using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Test;

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
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("userRepository");
		}
		#endregion

		#region GetUserInformation
		[TestMethod]
		public async Task GetUser_Should_Return_BadRequest_When_UserInformation_Is_Null()
		{
			// Arrange
			const UserInformation userInformation = null;
			UserController controller = CreateController();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.GetUserInformation(userInformation);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController();
			UserInformation userInformation = new UserInformation();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (ControllerFactory.NewContext(ControllerFactory.CreateModelValidator(s => false)))
			{
				HttpResponseMessage message = await controller.GetUserInformation(userInformation);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void GetUser_Should_Throw_Exception_When_User_By_Specified_UserGuid_Does_Not_Exist()
		{
			//Arrange
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Find(It.IsAny<Guid>())).Returns<Guid>(id => Task.FromResult<IUser>(null));

			UserController controller = CreateController(userRepositoryMock.Object);
			UserInformation userInformation = new UserInformation
			{
				UserKey = Guid.NewGuid()
			};

			// Act
			Func<Task<HttpResponseMessage>> func = () => controller.GetUserInformation(userInformation);

			// Assert
			func.ShouldThrow<HttpResponseException>()
				  .And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task GetUser_Should_Call_UserRepository_Find()
		{
			//Arrange
			bool called = false;
			IUser user = CreateUser();
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Find(It.IsAny<Guid>()))
						  .ReturnsAsync(user)
						  .Callback(() =>
							  {
								  called = true;
							  });

			UserController controller = CreateController(userRepositoryMock.Object);
			UserInformation userInformation = new UserInformation();

			// Act
			await controller.GetUserInformation(userInformation);

			// Assert
			called.Should().BeTrue();
		}

		[TestMethod]
		public async Task GetUser_Should_Pass_UserGuid_To_UserRepository_Find()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser user = CreateUser(userGuid);

			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Find(userGuid)).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserInformation userInformation = new UserInformation
			{
				UserKey = userGuid
			};

			// Act
			await controller.GetUserInformation(userInformation);

			// Assert
			userRepositoryMock.Verify(r => r.Find(userGuid), Times.Once());
		}

		[TestMethod]
		public async Task GetUser_Should_Return_User_By_Specified_UserGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser expected = CreateUser(userGuid);

			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Find(userGuid)).ReturnsAsync(expected);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserInformation userInformation = new UserInformation
			{
				UserKey = userGuid
			};

			// Act
			HttpResponseMessage message = await controller.GetUserInformation(userInformation);
			IUser actual = message.Content.GetValue<IUser>();

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region PutUserRegistration
		[TestMethod]
		public async Task PutUser_Should_Return_BadRequest_When_UserRegistration_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();
			const UserRegistration userRegistration = null;
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController();
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (ControllerFactory.NewContext(ControllerFactory.CreateModelValidator(s => false)))
			{
				HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Ok_When_Model_IsValid()
		{
			// Arrange
			IUser user = CreateUser();
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.OK;

			// Act
			using (ControllerFactory.NewContext())
			{
				HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public async Task PutUser_Should_Return_InternalError_When_User_Repository_Register_Returns_Null()
		{
			// Arrange
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(null);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.InternalServerError;

			// Act
			using (ControllerFactory.NewContext())
			{
				HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public async Task PutUser_Should_Call_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			bool called = false;
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>()))
							  .ReturnsAsync(It.IsAny<IUser>())
							  .Callback<string>(e =>
								  {
									  called = true;
									  e.Should().Be(email);
								  });

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			using (ControllerFactory.NewContext())
			{
				await controller.PutUserRegistration(userRegistration);

				// Assert
				called.Should().BeTrue();
			}
		}

		[TestMethod]
		public async Task PutUser_Should_Pass_Email_To_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(It.IsAny<IUser>());

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			using (ControllerFactory.NewContext())
			{
				await controller.PutUserRegistration(userRegistration);

				// Assert
				userRepositoryMock.Verify(r => r.Register(email), Times.Once());
			}
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Guid_Of_Created_User()
		{
			// Arrange
			Guid expected = Guid.NewGuid();
			IUser user = CreateUser(expected);

			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();

			// Act
			using (ControllerFactory.NewContext())
			{
				HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
				Guid actual = message.Content.GetValue<Guid>();

				// Assert
				actual.Should().Be(expected);
			}
		}
		#endregion

		#region Helper methods
		private static UserController CreateController(IUserRepository userRepository = null)
		{
			return ControllerFactory.Create<UserController>(userRepository ?? new Mock<IUserRepository>(MockBehavior.Strict).Object);
		}

		private static IUser CreateUser(Guid? userGuid = null)
		{
			var userMock = new Mock<IUser>();
			userMock.Setup(u => u.UserGuid).Returns(userGuid ?? Guid.NewGuid());
			return userMock.Object;
		}
		#endregion
	}
}