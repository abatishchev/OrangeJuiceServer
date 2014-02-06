﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class UserControllerTest
	{
		#region GetUser
		[TestMethod]
		public async Task GetUser_Should_Return_Status_NotFound_When_UserRepository_Returns_Null()
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
			Guid userId = Guid.NewGuid();
			IUser user = CreateUser(userId);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(userId)).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserId = userId };

			// Act
			await controller.GetUser(searchCriteria);

			// Assert
			repositoryMock.Verify(r => r.Search(userId), Times.Once);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_User_Returned_By_UserRepository_Search()
		{
			// Arrange
			Guid userId = Guid.NewGuid();
			IUser expected = CreateUser(userId);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Search(userId)).ReturnsAsync(expected);

			UserController controller = CreateController(repositoryMock.Object);
			UserSearchCriteria searchCriteria = new UserSearchCriteria { UserId = userId };

			// Act
			var result = (OkNegotiatedContentResult<IUser>)await controller.GetUser(searchCriteria);
			IUser actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_Status_Ok()
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
		public async Task PutUser_Should_Return_Status_InternalError_When_UserRepository_Register_Returns_Null()
		{
			// Arrange
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(null);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserModel());

			// Assert
			result.Should().BeOfType<InternalServerErrorResult>();
		}

		[TestMethod]
		public async Task PutUser_Should_Pass_Email_Name_To_UserRepository_Register()
		{
			// Arrange
			const string email = "email";
			const string name = "name";

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(email, name)).ReturnsAsync(Mock.Of<IUser>());

			UserController controller = CreateController(repositoryMock.Object);
			UserModel userModel = new UserModel { Email = email, Name = name };

			// Act
			await controller.PutUser(userModel);

			// Assert
			repositoryMock.Verify(r => r.Register(email, name), Times.Once);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Guid_Returned_By_UserRepository_Register()
		{
			// Arrange
			Guid expected = Guid.NewGuid();
			IUser user = CreateUser(expected);

			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserModel());
			Guid actual = ((OkNegotiatedContentResult<Guid>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Status_Ok()
		{
			// Arrange
			IUser user = CreateUser();
			var repositoryMock = new Mock<IUserRepository>();
			repositoryMock.Setup(r => r.Register(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

			UserController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PutUser(new UserModel());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<Guid>>();
		}
		#endregion

		#region Helper methods
		private static UserController CreateController(IUserRepository userRepository = null)
		{
			return ControllerFactory.Create<UserController>(userRepository ?? new Mock<IUserRepository>().Object);
		}

		private static IUser CreateUser(Guid? userId = null)
		{
			var userMock = new Mock<IUser>();
			userMock.Setup(u => u.UserId).Returns(userId ?? Guid.NewGuid());
			return userMock.Object;
		}
		#endregion
	}
}