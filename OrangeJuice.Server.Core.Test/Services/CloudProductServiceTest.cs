using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class CloudProductServiceTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_ProductRepository_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(Mock.Of<IProduct>());

			IProductService productService = CreateService(repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			repositoryMock.Verify(r => r.Search(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Pass_ProductId_Of_Product_Returned_By_ProductRepository_Search_To_AzureProductProvider_Get()
		{
			// Arrange
			Guid productId = Guid.NewGuid();
			IProduct product = CreateProduct(productId);
			IProductRepository repository = CreateRepository(product);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(repository, azureProviderMock.Object);

			// Act
			await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			azureProviderMock.Verify(p => p.Get(productId), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_ProductDescriptor_Returned_By_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Not_Null()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			Guid productId = Guid.NewGuid();
			IProduct product = CreateProduct(productId);
			IProductRepository repository = CreateRepository(product);

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(expected);

			IProductService productService = CreateService(repository, azureProviderMock.Object);

			// Act
			ProductDescriptor actual = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task Search_Should_Not_Call_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Not_Null()
		{
			// Arrange
			IProduct product = CreateProduct();
			IProductRepository repository = CreateRepository(product);

			var awsProvider = new Mock<IAwsProductProvider>();

			IProductService productService = CreateService(repository, awsProvider: awsProvider.Object);

			// Act
			await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			awsProvider.Verify(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>()), Times.Never);
		}

		[TestMethod]
		public async Task Search_Should_Not_Call_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			IProductRepository repository = CreateRepository(null);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(repository, azureProviderMock.Object);

			// Act
			await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			azureProviderMock.Verify(p => p.Get(It.IsAny<Guid>()), Times.Never);
		}

		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			IProductRepository repository = CreateRepository(null);

			IAwsProductProvider awsProvider = CreateAwsProvider(null);

			IProductService productService = CreateService(repository, awsProvider: awsProvider);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			Mock.Get(awsProvider).Verify(p => p.Search(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_Null_When_AwsProvider_Search_Returns_Null()
		{
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			IProductRepository repository = CreateRepository(null);

			var awsProvider = CreateAwsProvider(null);

			IProductService productService = CreateService(repository, awsProvider: awsProvider);

			// Act
			ProductDescriptor descriptor = await productService.Search(barcode, barcodeType);

			// Assert
			descriptor.Should().BeNull();
		}

		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_ProductRepository_Save_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(null);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType)).ReturnsAsync(Guid.NewGuid());

			IProductService productService = CreateService(repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			repositoryMock.Verify(r => r.Save(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Pass_ProductDescriptor_Returned_By_AwsProductProvider_Search_To_AzureProductProvider_Save_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			ProductDescriptor descriptor = new ProductDescriptor();

			IProductRepository repository = CreateRepository(null);

			IAzureProductProvider azureProvider = CreateAzureProvider(descriptor);

			var awsProvider = new Mock<IAwsProductProvider>();
			awsProvider.Setup(p => p.Search(barcode, barcodeType)).ReturnsAsync(descriptor);

			IProductService productService = CreateService(repository, azureProvider, awsProvider.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			Mock.Get(azureProvider).Verify(p => p.Save(descriptor), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Pass_Product_To_AzureProductProvider_Save_Having_ProductId_Returned_By_ProductRepository_Save_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			Guid productId = Guid.NewGuid();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(null);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType)).ReturnsAsync(productId);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(repositoryMock.Object, azureProviderMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			azureProviderMock.Verify(p => p.Save(It.Is<ProductDescriptor>(d => d.ProductId == productId)), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_ProductDescriptor_Returned_By_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			IProductRepository repository = CreateRepository(null);

			var awsProvider = CreateAwsProvider(expected);

			IProductService productService = CreateService(repository, awsProvider: awsProvider);

			// Act
			ProductDescriptor actual = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Get
		[TestMethod]
		public async Task Get_Should_Return_ProductDescriptor_Returned_By_AzureProductProvider_Get()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			Guid productId = Guid.NewGuid();
			IProduct product = CreateProduct(productId);
			IProductRepository repository = CreateRepository(product);

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(expected);

			IProductService productService = CreateService(repository, azureProviderMock.Object);

			// Act
			ProductDescriptor actual = await productService.Get(productId);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IProductService CreateService(IProductRepository repository, IAzureProductProvider azureProvider = null, IAwsProductProvider awsProvider = null)
		{
			return new CloudProductService(
				repository,
				azureProvider ?? CreateAzureProvider(),
				awsProvider ?? CreateAwsProvider(new ProductDescriptor()));
		}

		private static IProductRepository CreateRepository(IProduct product)
		{
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(product);
			return repositoryMock.Object;
		}

		private static IProduct CreateProduct(Guid? productId = null)
		{
			var productMock = new Mock<IProduct>();
			productMock.SetupGet(p => p.ProductId).Returns(productId ?? Guid.NewGuid());
			return productMock.Object;
		}

		private static IAzureProductProvider CreateAzureProvider(ProductDescriptor descriptor = null)
		{
			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Save(descriptor ?? It.IsAny<ProductDescriptor>())).Returns(Task.Delay(0));
			return azureProviderMock.Object;
		}

		private static IAwsProductProvider CreateAwsProvider(ProductDescriptor descriptor)
		{
			var awsProvider = new Mock<IAwsProductProvider>();
			awsProvider.Setup(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(descriptor);
			return awsProvider.Object;
		}
		#endregion
	}
}