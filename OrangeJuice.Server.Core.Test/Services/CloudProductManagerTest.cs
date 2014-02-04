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
	public class CloudProductManagerTest
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

			IProductManager manager = CreateManager(repositoryMock.Object);

			// Act
			await manager.Search(barcode, barcodeType);

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

			IProductManager manager = CreateManager(repository, azureProviderMock.Object);

			// Act
			await manager.Search("barcode", BarcodeType.EAN);

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

			IProductManager manager = CreateManager(repository, azureProviderMock.Object);

			// Act
			ProductDescriptor actual = await manager.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task Search_Should_Not_Call_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Not_Null()
		{
			// Arrange
			IProduct product = CreateProduct();
			IProductRepository repository = CreateRepository(product);

			var awsProviderMock = new Mock<IAwsProductProvider>();

			IProductManager manager = CreateManager(repository, awsProvider: awsProviderMock.Object);

			// Act
			await manager.Search("barcode", BarcodeType.EAN);

			// Assert
			awsProviderMock.Verify(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>()), Times.Never);
		}

		[TestMethod]
		public async Task Search_Should_Not_Call_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Null()
		{
			// Arrange
			IProductRepository repository = CreateRepository(null);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductManager manager = CreateManager(repository, azureProviderMock.Object);

			// Act
			await manager.Search("barcode", BarcodeType.EAN);

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

			var awsProviderMock = CreateAwsProvider(null);

			IProductManager manager = CreateManager(repository, awsProvider: awsProviderMock.Object);

			// Act
			await manager.Search(barcode, barcodeType);

			// Assert
			awsProviderMock.Verify(p => p.Search(barcode, barcodeType), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_Null_When_AwsProvider_Search_Returns_Null()
		{
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			IProductRepository repository = CreateRepository(null);

			var awsProviderMock = CreateAwsProvider(null);

			IProductManager manager = CreateManager(repository, awsProvider: awsProviderMock.Object);

			// Act
			ProductDescriptor descriptor = await manager.Search(barcode, barcodeType);

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

			IProductManager manager = CreateManager(repositoryMock.Object);

			// Act
			await manager.Search(barcode, barcodeType);

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

			var azureProviderMock = CreateAzureProvider(descriptor);

			var awsProvider = new Mock<IAwsProductProvider>();
			awsProvider.Setup(p => p.Search(barcode, barcodeType)).ReturnsAsync(descriptor);

			IProductManager manager = CreateManager(repository, azureProviderMock.Object, awsProvider.Object);

			// Act
			await manager.Search(barcode, barcodeType);

			// Assert
			azureProviderMock.Verify(p => p.Save(descriptor), Times.Once);
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

			IProductManager manager = CreateManager(repositoryMock.Object, azureProviderMock.Object);

			// Act
			await manager.Search(barcode, barcodeType);

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

			IProductManager manager = CreateManager(repository, awsProvider: awsProvider.Object);

			// Act
			ProductDescriptor actual = await manager.Search("barcode", BarcodeType.EAN);

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

			IProductManager manager = CreateManager(repository, azureProviderMock.Object);

			// Act
			ProductDescriptor actual = await manager.Get(productId);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IProductManager CreateManager(IProductRepository repository, IAzureProductProvider azureProvider = null, IAwsProductProvider awsProvider = null)
		{
			return new CloudProductManager(
				repository,
				azureProvider ?? CreateAzureProvider().Object,
				awsProvider ?? CreateAwsProvider(new ProductDescriptor()).Object);
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

		private static Mock<IAzureProductProvider> CreateAzureProvider(ProductDescriptor descriptor = null)
		{
			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Save(descriptor ?? It.IsAny<ProductDescriptor>())).Returns(Task.Delay(0));
			return azureProviderMock;
		}

		private static Mock<IAwsProductProvider> CreateAwsProvider(ProductDescriptor descriptor)
		{
			var awsProviderMock = new Mock<IAwsProductProvider>();
			awsProviderMock.Setup(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(descriptor);
			return awsProviderMock;
		}
		#endregion
	}
}