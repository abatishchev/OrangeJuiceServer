using System;
using System.Threading.Tasks;

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
		public async Task Search_Should_Not_Call_AwsProductProvider_Search_When_ProductRepository_Search_Returned_Not_Null()
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
		public async Task Search_Should_Not_Call_AzureProductProvider_Get_When_ProductRepository_Search_Returned_Null()
		{
			// Arrange
			const IProduct product = null;
			IProductRepository repository = CreateRepository(product);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			var awsProviderMock = CreateAwsProvider();

			IProductManager manager = CreateManager(repository, azureProviderMock.Object, awsProviderMock.Object);

			// Act
			await manager.Search("barcode", BarcodeType.EAN);

			// Assert
			azureProviderMock.Verify(p => p.Get(It.IsAny<Guid>()), Times.Never);
		}

		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_AwsProductProvider_Search_When_ProductRepository_Search_Returned_Null()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			const IProduct product = null;
			IProductRepository repository = CreateRepository(product);

			var awsProviderMock = CreateAwsProvider();

			IProductManager manager = CreateManager(repository, awsProvider: awsProviderMock.Object);

			// Act
			await manager.Search(barcode, barcodeType);

			// Assert
			awsProviderMock.Verify(p => p.Search(barcode, barcodeType), Times.Once);
		}

		#endregion

		#region Helper methods
		private static IProductManager CreateManager(IProductRepository repository = null, IAzureProductProvider azureProvider = null, IAwsProductProvider awsProvider = null)
		{
			return new CloudProductManager(
				repository ?? CreateRepository(CreateProduct()),
				azureProvider ?? Mock.Of<IAzureProductProvider>(),
				awsProvider ?? Mock.Of<IAwsProductProvider>());
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

		private static Mock<IAwsProductProvider> CreateAwsProvider(string barcode = "barcode", BarcodeType barcodeType = BarcodeType.EAN, ProductDescriptor descriptor = null)
		{
			var awsProviderMock = new Mock<IAwsProductProvider>();
			awsProviderMock.Setup(p => p.Search(barcode, barcodeType))
						   .ReturnsAsync(descriptor ?? new ProductDescriptor());
			return awsProviderMock;
		}
		#endregion
	}
}