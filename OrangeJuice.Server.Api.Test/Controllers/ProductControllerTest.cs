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
		#region PostTitle
		[TestMethod]
		public async Task PostTitle_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			ProductController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.PostTitle(new TitleSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PostTitle_Should_Pass_Title_To_ProductRepository_SearchTitle()
		{
			// Arrange
			const string title = "title";
			TitleSearchCriteria searchCriteria = new TitleSearchCriteria { Title = title };

			var productRepositoryMock = new Mock<IProductCoordinator>();
			productRepositoryMock.Setup(r => r.Search(title)).ReturnsAsync(new[] { new ProductDescriptor() });

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			await controller.PostTitle(searchCriteria);

			// Assert
			productRepositoryMock.Verify(r => r.Search(title), Times.Once);
		}

		[TestMethod]
		public async Task PostTitle_Should_Return_Collection_Of_ProductDescriptors_Returned_By_ProductRepository_SearchTitle()
		{
			// Arrange
			ProductDescriptor[] expected = { new ProductDescriptor() };

			var productRepositoryMock = new Mock<IProductCoordinator>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>())).ReturnsAsync(expected);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostTitle(new TitleSearchCriteria());
			var actual = ((OkNegotiatedContentResult<ProductDescriptor[]>)result).Content;

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region PostBarcode
		[TestMethod]
		public async Task PostBarcode_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			ProductController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.PostBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PostBarcode_Should_Pass_Title_To_ProductRepository_Lookup()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var productRepositoryMock = new Mock<IProductCoordinator>();
			productRepositoryMock.Setup(r => r.Lookup(barcode, barcodeType)).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			await controller.PostBarcode(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			productRepositoryMock.Verify(r => r.Lookup(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task PostBarcode_Should_Return_ProductDescriptor_Returned_By_ProductRepository_Lookup()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var productRepositoryMock = new Mock<IProductCoordinator>();
			productRepositoryMock.Setup(r => r.Lookup(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostBarcode(new BarcodeSearchCriteria());
			ProductDescriptor actual = ((OkNegotiatedContentResult<ProductDescriptor>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task PostBarcode_Should_Return_Null_When_ProductRepository_Lookup_Returned_Null()
		{
			// Arrange
			var productRepositoryMock = new Mock<IProductCoordinator>();
			productRepositoryMock.Setup(r => r.Lookup(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostBarcode(new BarcodeSearchCriteria());
			ProductDescriptor actual = ((OkNegotiatedContentResult<ProductDescriptor>)result).Content;

			// Assert
			actual.Should().BeNull();
		}
		#endregion

		#region Helper methods
		private static ProductController CreateController(IProductCoordinator coordinator = null)
		{
			return ControllerFactory.Create<ProductController>(coordinator ?? new Mock<IProductCoordinator>().Object);
		}
		#endregion
	}
}