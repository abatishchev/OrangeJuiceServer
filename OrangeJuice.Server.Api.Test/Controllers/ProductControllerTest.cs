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
	public class ProductControllerTest
	{
		#region GetProduct
		[TestMethod]
		public async Task GetProduct_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			ProductController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.GetProduct(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetProduct_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			await controller.GetProduct(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			productRepositoryMock.Verify(r => r.Search(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task GetProduct_Should_Return_ProductDescriptor_Returned_By_ProductManager_Search()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProduct(new BarcodeSearchCriteria());
			ProductDescriptor actual = ((OkNegotiatedContentResult<ProductDescriptor>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetProduct_Should_Return_NotFound_When_ProductManager_Search_Returns_Null()
		{
			// Arrange
			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProduct(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}
		#endregion

		#region Helper methods
		private static ProductController CreateController(IProductManager manager = null)
		{
			return ControllerFactory.Create<ProductController>(manager ?? new Mock<IProductManager>().Object);
		}
		#endregion
	}
}