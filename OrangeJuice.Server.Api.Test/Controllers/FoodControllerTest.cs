using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class FoodControllerTest
	{
		#region Ctor
		public void Ctor_Should_Throw_Exception_When_AwsClientFactory_Is_Null()
		{
			// Arange
			const AwsClientFactory awsClientFactory = null;
			const GroceryDescriptionFactory groceryDescriptionFactor = null;

			// Act
			Action action = () => new FoodController(awsClientFactory, groceryDescriptionFactor);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsClientFactory");
		}

		public void Ctor_Should_Throw_Exception_When_GroceryDescriptionFactory_Is_Null()
		{
			// Arange
			AwsClientFactory awsClientFactory = new Mock<AwsClientFactory>().Object;
			const GroceryDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new FoodController(awsClientFactory, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsClientFactory");
		}
		#endregion

		[TestMethod]
		public void GetDescription_Should_()
		{
			// TODO: tests
		}
	}
}