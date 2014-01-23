using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class FoodControllerTest
	{
		#region GetDescription
		[TestMethod]
		public async Task GetDescription_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController(exception: new ArgumentNullException());
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();

			// Act
			IHttpActionResult result = await controller.GetDescription(searchCriteria);

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetDescription_Should_Pass_Title_To_FoodRepository_SearchByTitle()
		{
			// Arrange
			const string title = "title";

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Search(title)).ReturnsAsync(new[] { new FoodDescription() });

			FoodController controller = CreateController(foodRepositoryMock.Object);
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria { Title = title };

			// Act
			await controller.GetDescription(searchCriteria);

			// Assert
			foodRepositoryMock.Verify(r => r.Search(title), Times.Once);
		}

		[TestMethod]
		public async Task GetDescription_Should_Return_Description_Of_Found_Food()
		{
			// Arrange
			FoodDescription[] expected = { new FoodDescription() };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Search(It.IsAny<string>())).ReturnsAsync(expected);

			FoodController controller = CreateController(foodRepositoryMock.Object);
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();

			// Act
			IHttpActionResult result = await controller.GetDescription(searchCriteria);
			var actual = ((OkNegotiatedContentResult<ICollection<FoodDescription>>)result).Content;

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static FoodController CreateController(IFoodRepository repository = null, Exception exception = null)
		{
			var controller = ControllerFactory.Create<FoodController>(repository ?? new Mock<IFoodRepository>().Object);
			if (exception != null)
				controller.ModelState.AddModelError("", exception);
			return controller;
		}
		#endregion
	}
}