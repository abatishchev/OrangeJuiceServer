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
		#region GetByTitle
		[TestMethod]
		public async Task GetByTitle_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.GetByTitle(new TitleSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetByTitle_Should_Pass_Title_To_FoodRepository_SearchByTitle()
		{
			// Arrange
			const string title = "title";
			TitleSearchCriteria searchCriteria = new TitleSearchCriteria { Title = title };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.SearchByTitle(title)).ReturnsAsync(new[] { new FoodDescription() });

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			await controller.GetByTitle(searchCriteria);

			// Assert
			foodRepositoryMock.Verify(r => r.SearchByTitle(title), Times.Once);
		}

		[TestMethod]
		public async Task GetByTitle_Should_Return_Collection_Of_FoodDescription_Returned_By_FoodRepository_SearchByTitle()
		{
			// Arrange
			FoodDescription[] expected = { new FoodDescription() };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.SearchByTitle(It.IsAny<string>())).ReturnsAsync(expected);

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetByTitle(new TitleSearchCriteria());
			var actual = ((OkNegotiatedContentResult<ICollection<FoodDescription>>)result).Content;

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region GetByBarcode
		[TestMethod]
		public async Task GetByBarcode_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.GetByBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetByBarcode_Should_Pass_Title_To_FoodRepository_SearchByBarcode()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.SearchByBarcode(barcode, barcodeType)).ReturnsAsync(new[] { new FoodDescription() });

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			await controller.GetByBarcode(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			foodRepositoryMock.Verify(r => r.SearchByBarcode(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task GetByBarcode_Should_Return_Collection_Of_FoodDescription_Returned_By_FoodRepository_SearchByBarcode()
		{
			// Arrange
			FoodDescription[] expected = { new FoodDescription() };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.SearchByBarcode(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetByBarcode(new BarcodeSearchCriteria());
			var actual = ((OkNegotiatedContentResult<ICollection<FoodDescription>>)result).Content;

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static FoodController CreateController(IFoodRepository repository = null)
		{
			return ControllerFactory.Create<FoodController>(repository ?? new Mock<IFoodRepository>().Object);
		}
		#endregion
	}
}