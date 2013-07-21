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
	}
}