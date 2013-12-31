using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class UserControllerTest
	{
		#region GetUser
		[TestMethod]
		public async Task GetUser_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController(exception: new ArgumentNullException());
			UserSearchCriteria searchCriteria = new UserSearchCriteria();

			// Act
			IHttpActionResult result = await controller.GetUserInformation(searchCriteria);

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetUser_Should_Return_NotFound_When_User_By_Specified_Guid_Does_Not_Exist()
		{
			//Arrange
			var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
			userRepositoryMock.Setup(r => r.SearchByGuid(It.IsAny<Guid>())).ReturnsAsync(null);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserGuid = Guid.NewGuid() };

			// Act
			IHttpActionResult result = await controller.GetUserInformation(searchCriteria);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
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
			userRepositoryMock.Verify(r => r.SearchByGuid(userGuid), Times.Once());
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
			var result = (OkNegotiatedContentResult<IUser>)await controller.GetUserInformation(searchCriteria);
			IUser actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_Ok()
		{
			// Arrange
			IUser user = CreateUser();
			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.SearchByGuid(It.IsAny<Guid>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria();

			// Act
			IHttpActionResult result = await controller.GetUserInformation(searchCriteria);

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<IUser>>();
		}
		#endregion

		#region PutUser
		[TestMethod]
		public async Task PutUser_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController(exception: new ArgumentNullException());
			UserRegistration userRegistration = new UserRegistration();

			// Act
			IHttpActionResult result = await controller.PutUserRegistration(userRegistration);

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PutUser_Should_Return_InternalError_When_User_Repository_Register_Returns_Null()
		{
			// Arrange
			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(null);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();

			// Act
			IHttpActionResult result = await controller.PutUserRegistration(userRegistration);

			// Assert
			result.Should().BeOfType<InternalServerErrorResult>();
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
			userRepositoryMock.Verify(r => r.Register(email), Times.Once());
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Guid_Returned_By_UserRepository_Register()
		{
			// Arrange
			Guid expected = Guid.NewGuid();
			IUser user = CreateUser(expected);

			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();

			// Act
			var result = (OkNegotiatedContentResult<Guid>)await controller.PutUserRegistration(userRegistration);
			Guid actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Ok()
		{
			// Arrange
			IUser user = CreateUser();
			var userRepositoryMock = new Mock<IUserRepository>();
			userRepositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(userRepositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration();

			// Act
			IHttpActionResult result = await controller.PutUserRegistration(userRegistration);

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<Guid>>();
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