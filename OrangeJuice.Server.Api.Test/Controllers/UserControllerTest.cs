using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Web;
using Xunit;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	public class UserControllerTest
	{
		#region GetUser
		[Fact]
		public void GetUser_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();

			// Act
			Func<Task> task = () => controller.GetUser(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task GetUser_Should_Return_Status_NoContent_When_UserRepository_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>())).ReturnsAsync(null);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetUser(new UserSearchCriteria());

			// Assert
			result.Should().BeOfType<StatusCodeResult>()
				  .Which.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task GetUser_Should_Pass_UserGuid_To_UserRepository_Search()
		{
			// Arrange
			Guid userId = Guid.NewGuid();
			User user = CreateUser(userId);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(userId)).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.GetUser(new UserSearchCriteria { UserId = userId });

			// Assert
			repositoryMock.VerifyAll();
		}

		[Fact]
		public async Task GetUser_Should_Return_User_Returned_By_UserRepository_Search()
		{
			// Arrange
			Guid userId = Guid.NewGuid();
			User expected = CreateUser(userId);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(userId)).ReturnsAsync(expected);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			var result = await controller.GetUser(new UserSearchCriteria { UserId = userId });
			User actual = ((OkNegotiatedContentResult<User>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public async Task GetUser_Should_Return_Status_Ok()
		{
			// Arrange
			User user = CreateUser();
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetUser(new UserSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<User>>();
		}
		#endregion

		#region PutUser
		[Fact]
		public void PutUser_Should_Should_Throw_Exception_When_UserModel_Is_Null()
		{
			// Arrange
			UserController controller = CreateController();

			// Act
			Func<Task> task = () => controller.PutUser(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task PutUser_Should_Pass_Email_Name_To_UserRepository_Register()
		{
			// Arrange
			const string email = "email";
			const string name = "name";

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(email, name)).ReturnsAsync(new User());

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.PutUser(new UserModel { Email = email, Name = name });

			// Assert
			repositoryMock.VerifyAll();
		}

		[Fact]
		public async Task PutUser_Should_Return_User_Returned_By_UserRepository_Register()
		{
			// Arrange
			User expected = CreateUser();

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expected);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserModel());
			User actual = ((CreatedNegotiatedContentResult<User>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public async Task PutUser_Should_Return_Status_Created()
		{
			// Arrange
			User user = CreateUser();
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserModel());

			// Assert
			result.Should().BeOfType<CreatedNegotiatedContentResult<User>>();
		}
		#endregion

		#region Helper methods
		private static UserController CreateController(IUserRepository userRepository = null)
		{
			return ControllerFactory<UserController>.Create(
				userRepository ?? Mock.Of<IUserRepository>(),
				CreateUrlProvider());
		}

		private static IUrlProvider CreateUrlProvider()
		{
			var providerMock = new Mock<IUrlProvider>();
			providerMock.Setup(p => p.UriFor(It.IsAny<Expression<Action<UserController>>>()))
						.Returns(new Uri("http://example.com"));
			return providerMock.Object;
		}

		private static User CreateUser(Guid? userId = null)
		{
			return new User
			{
				UserId = userId ?? Guid.NewGuid()
			};
		}
		#endregion
	}
}