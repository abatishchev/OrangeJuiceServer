using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class FoodControllerTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsOptions_Is_Null()
		{
			// Arrange
			const AwsOptions awsOptions = null;
			const GroceryDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new FoodController(awsOptions, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsOptions");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_GroceryDescriptionFactorys_Is_Null()
		{
			// Arrange
			AwsOptions awsOptions = new AwsOptions();
			const GroceryDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new FoodController(awsOptions, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("groceryDescriptionFactory");
		}
		#endregion


		#region GetDescription
		[TestMethod]
		public void GetDescription_Should_Bla()
		{
			// TODO: tests
		}
		#endregion
	}
}