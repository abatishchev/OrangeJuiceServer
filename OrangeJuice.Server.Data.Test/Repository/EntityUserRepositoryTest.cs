﻿using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Test.Repository
{
	[TestClass]
	public class EntityUserRepositoryTest
	{
		#region Register
		[TestMethod]
		public async Task Register_Should_Return_User_Having_Properties()
		{
			// Arrange
			const string email = "email";
			const string name = "name";

			var userUnitMock = new Mock<IUserUnit>();
			userUnitMock.Setup(u => u.Add(It.IsAny<User>())).Returns<User>(Task.FromResult);

			IUserRepository repository = CreateRepository(userUnitMock.Object);

			// Act
			IUser user = await repository.Register(email, name);

			// Assert
			user.Email.Should().NotBeEmpty();
			user.Name.Should().NotBeEmpty();
		}

		[TestMethod]
		public async Task Register_Should_Call_UserUnit_Add()
		{
			// Arrange
			var userUnitMock = new Mock<IUserUnit>();
			userUnitMock.Setup(u => u.Add(It.IsAny<User>())).Returns<User>(Task.FromResult);

			IUserRepository repository = CreateRepository(userUnitMock.Object);

			// Act
			User user = (User)(await repository.Register("email", "name"));

			// Assert
			userUnitMock.Verify(u => u.Add(user), Times.Once);
		}
		#endregion

		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_UserGuid_To_UserUnit_Get()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();

			var userUnitMock = new Mock<IUserUnit>();

			IUserRepository repository = CreateRepository(userUnitMock.Object);

			// Act
			await repository.Search(userGuid);

			// Assert
			userUnitMock.Verify(u => u.Get(userGuid), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Returned_User_Returned_By_UserUnit_Get()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			User expected = new User();

			var userUnitMock = new Mock<IUserUnit>();
			userUnitMock.Setup(u => u.Get(userGuid)).ReturnsAsync(expected);

			IUserRepository repository = CreateRepository(userUnitMock.Object);

			// Act
			IUser actual = await repository.Search(userGuid);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Dispose
		[TestMethod]
		public void Dispose_Should_Call_UserUnit_Dispose()
		{
			// Arrange
			var userUnitMock = new Mock<IUserUnit>();

			IUserRepository repository = CreateRepository(userUnitMock.Object);

			// Act
			repository.Dispose();

			// Assert
			userUnitMock.Verify(u => u.Dispose(), Times.Once);
		}
		#endregion

		#region Helper methods
		private static IUserRepository CreateRepository(IUserUnit userUnit = null)
		{
			return new EntityUserRepository(userUnit ?? new Mock<IUserUnit>().Object);
		}

		#endregion
	}
}