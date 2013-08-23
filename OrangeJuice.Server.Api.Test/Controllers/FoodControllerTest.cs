using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ModelBinding;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;
using OrangeJuice.Server.Api.Validation;
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
			const IAwsClientFactory awsClientFactory = null;
			const IGroceryDescriptionFactory groceryDescriptionFactory = null;

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
			IAwsClientFactory awsClientFactory = new Mock<IAwsClientFactory>().Object;
			const IGroceryDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new FoodController(awsClientFactory, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("groceryDescriptionFactory");
		}
		#endregion

		#region GetDescription
		[TestMethod]
		public async void GetDescription_Should_Return_BadRequest_When_SearchCriteria_Is_Null()
		{
			// Arrange
			FoodController controller = CreateController();
			const GrocerSearchCriteria searchCriteria = null;
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.GetDescription(searchCriteria);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async void GetDescription_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController();
			GrocerSearchCriteria searchCriteria = new GrocerSearchCriteria();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (ControllerFactory.NewContext())
			{
				HttpResponseMessage message = await controller.GetDescription(searchCriteria);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		#endregion

		#region Helper methods
		private static FoodController CreateController(IAwsClientFactory awsClientFactory = null, IGroceryDescriptionFactory groceryDescriptionFactory = null)
		{
			return ControllerFactory.Create<FoodController>(
				awsClientFactory ?? new Mock<IAwsClientFactory>(MockBehavior.Strict).Object,
				groceryDescriptionFactory ?? new Mock<IGroceryDescriptionFactory>(MockBehavior.Strict).Object);
		}
		#endregion
	}
}