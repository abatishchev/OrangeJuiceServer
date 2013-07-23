using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Data.Model.Test.Entities
{
	[TestClass]
	public class UserTest
	{
		[TestMethod]
		public void CreateNew_Should_Assign_UserGuid()
		{
			// Arrange
			// Act
			User user = User.CreateNew();

			// Assert
			user.UserGuid.Should().NotBeEmpty();
		}

		[TestMethod]
		public void UserGuid_Should_Be_Unique()
		{
			// Arrange
			// Act
			Guid guid1 = User.CreateNew().UserGuid;
			Guid guid2 = User.CreateNew().UserGuid;

			// Assert
			guid1.Should().NotBe(guid2);
		}

		[TestMethod]
		public void CreateNew_Accepting_Null_Email_Should_Not_Assign_Email()
		{
			// Arrange
			// Act
			User user = User.CreateNew(email: null);

			// Assert
			user.Email.Should().BeNull();
		}

		[TestMethod]
		public void CreateNew_Accepting_Not_Null_Email_Should_Assign_Email()
		{
			// Arrange
			const string email = "test@example.com";

			// Act
			User user = User.CreateNew(email: email);

			// Assert
			user.Email.Should().Be(email);
		}
	}
}