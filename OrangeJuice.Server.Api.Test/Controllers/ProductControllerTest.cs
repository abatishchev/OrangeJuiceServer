﻿using System;
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
		#region GetProductId
		[TestMethod]
		public async Task GetProducId_Should_Return_Status_Ok()
		{
			// Arrange
			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<ProductDescriptor>>();
		}

		[TestMethod]
		public async Task GetProducId_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search()
		{
			// Arrange
			Guid productId = Guid.NewGuid();

			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Get(productId)).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			await controller.GetProductId(new ProductSearchCriteria { ProductId = productId });

			// Assert
			productRepositoryMock.Verify(r => r.Get(productId), Times.Once);
		}

		[TestMethod]
		public async Task GetProducId_Should_Return_ProductDescriptor_Returned_By_ProductManager_Search()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(expected);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());
			ProductDescriptor actual = ((OkNegotiatedContentResult<ProductDescriptor>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetProducId_Should_Return_Status_NotFound_When_ProductManager_Search_Returns_Null()
		{
			// Arrange
			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductId(new ProductSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}
		#endregion

		#region GetProductBarcode
		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_Ok()
		{
			// Arrange
			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<ProductDescriptor>>();
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Pass_Barcode_BarcodeType_To_ProductManager_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new ProductDescriptor());

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			await controller.GetProductBarcode(new BarcodeSearchCriteria { Barcode = barcode, BarcodeType = barcodeType });

			// Assert
			productRepositoryMock.Verify(r => r.Search(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Return_ProductDescriptor_Returned_By_ProductManager_Search()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(expected);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());
			ProductDescriptor actual = ((OkNegotiatedContentResult<ProductDescriptor>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_NotFound_When_ProductManager_Search_Returns_Null()
		{
			// Arrange
			var productRepositoryMock = new Mock<IProductManager>();
			productRepositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(null);

			ProductController controller = CreateController(productRepositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetProductBarcode(new BarcodeSearchCriteria());

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