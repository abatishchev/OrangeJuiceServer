using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class FoodControllerTest
	{
		#region PostTitle
		[TestMethod]
		public async Task PostTitle_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.PostTitle(new TitleSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PostTitle_Should_Pass_Title_To_FoodRepository_SearchTitle()
		{
			// Arrange
			const string title = "title";
			TitleSearchCriteria searchCriteria = new TitleSearchCriteria { Title = title };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Search(title)).ReturnsAsync(new[] { new FoodDescriptor() });

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			await controller.PostTitle(searchCriteria);

			// Assert
			foodRepositoryMock.Verify(r => r.Search(title), Times.Once);
		}

		[TestMethod]
		public async Task PostTitle_Should_Return_Collection_Of_FoodDescriptors_Returned_By_FoodRepository_SearchTitle()
		{
			// Arrange
			FoodDescriptor[] expected = { new FoodDescriptor() };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Search(It.IsAny<string>())).ReturnsAsync(expected);

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostTitle(new TitleSearchCriteria());
			var actual = ((OkNegotiatedContentResult<FoodDescriptor[]>)result).Content;

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region PostBarcode
		[TestMethod]
		public async Task PostBarcode_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.PostBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PostBarcode_Should_Pass_Title_To_FoodRepository_Lookup()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Lookup(barcode, barcodeType)).ReturnsAsync(new FoodDescriptor());

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			await controller.PostBarcode(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			foodRepositoryMock.Verify(r => r.Lookup(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task PostBarcode_Should_Return_FoodDescriptor_Returned_By_FoodRepository_Lookup()
		{
			// Arrange
			FoodDescriptor expected = new FoodDescriptor();

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Lookup(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostBarcode(new BarcodeSearchCriteria());
			FoodDescriptor actual = ((OkNegotiatedContentResult<FoodDescriptor>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PostBarcode_Should_Return_Null_When_FoodRepository_Lookup_Returned_Null()
		{
			// Arrange
			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.Lookup(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			FoodController controller = CreateController(foodRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostBarcode(new BarcodeSearchCriteria());
			FoodDescriptor actual = ((OkNegotiatedContentResult<FoodDescriptor>)result).Content;

			// Assert
			actual.Should().BeNull();
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