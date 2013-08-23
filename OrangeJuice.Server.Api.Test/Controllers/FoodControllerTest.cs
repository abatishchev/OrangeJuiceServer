using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class FoodControllerTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsClientFactory_Is_Null()
		{
			// Arrange
			const AwsClientFactory awsClientFactory = null;
			const GroceryDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new FoodController(awsClientFactory, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsClientFactory");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_GroceryDescriptionFactorys_Is_Null()
		{
			// Arrange
			AwsClientFactory awsClientFactory = new AwsClientFactory(new AwsOptions(), new Mock<IUrlEncoder>().Object, new Mock<IDateTimeProvider>().Object);
			const GroceryDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new FoodController(awsClientFactory, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("groceryDescriptionFactory");
		}
		#endregion

		#region GetDescription
		[TestMethod]
		public void GetDescription_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}
		#endregion
	}
}