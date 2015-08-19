using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Ab.Amazon.Data;
using AwsProduct = Ab.Amazon.Data.Product;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Controllers;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	public class ProductControllerTest
	{
		#region GetProductId
		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public void GetProducId_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null(Type type)
		{
			// Arrange
			IProductController controller = CreateController(type);

			// Act
			Func<Task> task = () => controller.GetProductId(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProducId_Should_Return_Status_Ok(Type type)
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new AwsProduct());

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<AwsProduct>>();
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProducId_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search(Type type)
		{
			// Arrange
			Guid productId = Guid.NewGuid();

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Get(productId)).ReturnsAsync(new AwsProduct());

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			await controller.GetProductId(new ProductSearchCriteria { ProductId = productId });

			// Assert
			serviceMock.Verify(r => r.Get(productId), Times.Once);
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProducId_Should_Return_AwsProduct_Returned_By_ProductManager_Search(Type type)
		{
			// Arrange
			AwsProduct expected = new AwsProduct();

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(expected);

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());
			AwsProduct actual = ((OkNegotiatedContentResult<AwsProduct>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProducId_Should_Return_Status_NoContent_When_ProductManager_Search_Returns_Null(Type type)
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());

			// Assert
			result.Should().BeOfType<StatusCodeResult>()
				  .Which.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
		#endregion

		#region GetProductBarcode
		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public void GetProducBarcode_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null(Type type)
		{
			// Arrange
			IProductController controller = CreateController(type);

			// Act
			Func<Task> task = () => controller.GetProductBarcode(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProductBarcode_Should_Return_Status_Ok(Type type)
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new AwsProduct[0]);

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<AwsProduct[]>>();
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProductBarcode_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search(Type type)
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new AwsProduct[0]);

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			await controller.GetProductBarcode(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			serviceMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProductBarcode_Should_Return_AwsProducts_Returned_By_ProductManager_Search(Type type)
		{
			// Arrange
			var expected = new[] { new AwsProduct(), new AwsProduct() };

			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());
			var actual = ((OkNegotiatedContentResult<AwsProduct[]>)result).Content;

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Theory]
		[InlineData(typeof(ProductController))]
		[InlineData(typeof(FSharp.Controllers.ProductController))]

		public async Task GetProductBarcode_Should_Return_Status_NoContent_When_ProductManager_Search_Returns_Null(Type type)
		{
			// Arrange
			var serviceMock = new Mock<IProductService>();
			serviceMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			IProductController controller = CreateController(type, serviceMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<StatusCodeResult>()
				  .Which.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}
		#endregion

		#region Helper methods
		private static IProductController CreateController(Type type, IProductService service = null)
		{
			return (IProductController)ControllerFactory.Create(type, service ?? new Mock<IProductService>().Object);
		}
		#endregion
	}
}