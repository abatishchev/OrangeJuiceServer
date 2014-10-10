using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	public class ProductControllerTest
	{
		#region GetProductId
		[Fact]
		public void GetProducId_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null()
		{
			// Arrange
			ProductController controller = CreateController();

			// Act
			Func<Task> task = () => controller.GetProductId(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task GetProducId_Should_Return_Status_Ok()
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<ProductDescriptor>>();
		}

		[Fact]
		public async Task GetProducId_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search()
		{
			// Arrange
			Guid productId = Guid.NewGuid();

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Get(productId)).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			await controller.GetProductId(new ProductSearchCriteria { ProductId = productId });

			// Assert
			serviceMock.Verify(r => r.Get(productId), Times.Once);
		}

		[Fact]
		public async Task GetProducId_Should_Return_ProductDescriptor_Returned_By_ProductManager_Search()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(expected);

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());
			ProductDescriptor actual = ((OkNegotiatedContentResult<ProductDescriptor>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public async Task GetProducId_Should_Return_Status_NoContent_When_ProductManager_Search_Returns_Null()
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());

			// Assert
			result.Should().BeOfType<StatusCodeResult>()
				  .Which.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
		#endregion

		#region GetProductBarcode
		[Fact]
		public void GetProducBarcode_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null()
		{
			// Arrange
			ProductController controller = CreateController();

			// Act
			Func<Task> task = () => controller.GetProductBarcode(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_Ok()
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new ProductDescriptor[0]);

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<ProductDescriptor[]>>();
		}

		[Fact]
		public async Task GetProductBarcode_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new ProductDescriptor[0]);

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			await controller.GetProductBarcode(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			serviceMock.VerifyAll();
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_ProductDescriptors_Returned_By_ProductManager_Search()
		{
			// Arrange
			var expected = new[] { new ProductDescriptor(), new ProductDescriptor() };

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());
			var actual = ((OkNegotiatedContentResult<ProductDescriptor[]>)result).Content;

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_NoContent_When_ProductManager_Search_Returns_Null()
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			ProductController controller = CreateController(serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<StatusCodeResult>()
				  .Which.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
		#endregion

		#region Helper methods
		private static ProductController CreateController(IProductService service = null)
		{
			return ControllerFactory<ProductController>.Create(service ?? new Mock<IProductService>().Object);
		}
		#endregion
	}
}