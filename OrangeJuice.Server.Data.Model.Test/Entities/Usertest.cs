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
	}
}