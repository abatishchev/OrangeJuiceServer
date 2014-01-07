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
			UserController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			UserSearchCriteria searchCriteria = new UserSearchCriteria();

			// Act
			IHttpActionResult result = await controller.GetUser(searchCriteria);

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetUser_Should_Return_NotFound_When_UserRepository_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>())).ReturnsAsync(null);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetUser(new UserSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[TestMethod]
		public async Task GetUser_Should_Pass_UserGuid_To_UserRepository_Search()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser user = CreateUser(userGuid);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(userGuid)).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserGuid = userGuid };

			// Act
			await controller.GetUser(searchCriteria);

			// Assert
			repositoryMock.Verify(r => r.Search(userGuid), Times.Once());
		}

		[TestMethod]
		public async Task GetUser_Should_Return_User_By_Specified_UserGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			IUser expected = CreateUser(userGuid);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(userGuid)).ReturnsAsync(expected);

			UserController controller = CreateController(repositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserGuid = userGuid };

			// Act
			var result = (OkNegotiatedContentResult<IUser>)await controller.GetUser(searchCriteria);
			IUser actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_Ok()
		{
			// Arrange
			IUser user = CreateUser();
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria();

			// Act
			IHttpActionResult result = await controller.GetUser(searchCriteria);

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<IUser>>();
		}
		#endregion

		#region PutUser
		[TestMethod]
		public async Task PutUser_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			UserController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.PutUser(new UserRegistration());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PutUser_Should_Return_InternalError_When_UserRepository_Register_Returns_Null()
		{
			// Arrange
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(null);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserRegistration());

			// Assert
			result.Should().BeOfType<InternalServerErrorResult>();
		}

		[TestMethod]
		public async Task PutUser_Should_Pass_Email_To_UserRepository_Register()
		{
			// Arrange
			const string email = "test@example.com";
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(email)).ReturnsAsync(new Mock<IUser>().Object);

			UserController controller = CreateController(repositoryMock.Object);
			UserRegistration userRegistration = new UserRegistration { Email = email };

			// Act
			await controller.PutUser(userRegistration);

			// Assert
			repositoryMock.Verify(r => r.Register(email), Times.Once());
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Guid_Returned_By_UserRepository_Register()
		{
			// Arrange
			Guid expected = Guid.NewGuid();
			IUser user = CreateUser(expected);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			var result = (OkNegotiatedContentResult<Guid>)await controller.PutUser(new UserRegistration());
			Guid actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Ok()
		{
			// Arrange
			IUser user = CreateUser();
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserRegistration());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<Guid>>();
		}
		#endregion

		#region Helper methods
		private static UserController CreateController(IUserRepository userRepository = null)
		{
			return ControllerFactory.Create<UserController>(userRepository ?? new Mock<IUserRepository>().Object);
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