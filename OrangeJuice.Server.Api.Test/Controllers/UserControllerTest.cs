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
		public async Task GetUser_Should_Return_Status_BadRequest_When_SearchCriteria_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();
			const UserSearchCriteria searchCriteria = null;
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.GetUserInformation(searchCriteria);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_Message_Having_Exception_Set_When_SearchCriteria_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();
			const UserSearchCriteria searchCriteria = null;

			// Act
			HttpResponseMessage message = await controller.GetUserInformation(searchCriteria);

			// Assert
			ObjectContent<HttpError> content = message.Content as ObjectContent<HttpError>;
			Action action = () => { throw content.GetException(); };

			action.ShouldThrow<ArgumentNullException>();
		}

		[TestMethod]
		public async Task GetUser_Should_Return_Status_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController(exception: new ArgumentNullException());
			UserSearchCriteria searchCriteria = new UserSearchCriteria();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.GetUserInformation(searchCriteria);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void GetUser_Should_Throw_Exception_When_User_By_Specified_Guid_Does_Not_Exist()
		{
			//Arrange
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.SearchByGuid(It.IsAny<Guid>())).ReturnsAsync(null);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserGuid = Guid.NewGuid() };

			// Act
			Func<Task<HttpResponseMessage>> func = () => controller.GetUserInformation(searchCriteria);

			// Assert
			func.ShouldThrow<HttpResponseException>()
				.And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task GetUser_Should_Pass_UserGuid_To_UserRepository_SearchByGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser user = CreateUser(userGuid);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.SearchByGuid(userGuid)).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserGuid = userGuid };

			// Act
			await controller.GetUserInformation(searchCriteria);

			// Assert
			userRepositoryMock.Verify(r => r.SearchByGuid(userGuid), Times.Once);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_User_By_Specified_UserGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser expected = CreateUser(userGuid);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.SearchByGuid(userGuid)).ReturnsAsync(expected);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserGuid = userGuid };

			// Act
			HttpResponseMessage message = await controller.GetUserInformation(searchCriteria);
			IUser actual = message.Content.GetValue<IUser>();

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region PutUserRegistration
		[TestMethod]
		public async Task PutUser_Should_Return_Status_BadRequest_When_UserRegistration_Is_Null()
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
		public async Task PutUser_Should_Return_Message_Having_Exception_Set_When_UserRegistration_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();
			const UserRegistration userRegistration = null;

			// Act
			HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);

			// Assert
			ObjectContent<HttpError> content = message.Content as ObjectContent<HttpError>;
			Action action = () => { throw content.GetException(); };

			action.ShouldThrow<ArgumentNullException>();
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Status_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController(exception: new ArgumentNullException());
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Status_InternalError_When_User_Repository_Register_Returns_Null()
		{
			// Arrange
			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(null);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.InternalServerError;

			// Act
			HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Pass_Email_To_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(email)).ReturnsAsync(new Mock<IUser>().Object);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			await controller.PutUserRegistration(userRegistration);

			// Assert
			userRepositoryMock.Verify(r => r.Register(email), Times.Once);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Guid_Of_Created_User()
		{
			// Arrange
			Guid expected = Guid.NewGuid();
			IUser user = CreateUser(expected);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();

			// Act
			HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
			Guid actual = message.Content.GetValue<Guid>();

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Status_Created_When_User_Registered()
		{
			// Arrange
			IUser user = CreateUser();

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();
			const HttpStatusCode expected = HttpStatusCode.Created;

			// Act
			HttpResponseMessage message = await controller.PutUserRegistration(userRegistration);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static UserController CreateController(IUserRepository userRepository = null, Exception exception = null)
		{
			var controller = ControllerFactory.Create<UserController>(userRepository ?? new Mock<IUserRepository>(MockBehavior.Strict).Object);
			if (exception != null)
				controller.ModelState.AddModelError("", exception);
			return controller;
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