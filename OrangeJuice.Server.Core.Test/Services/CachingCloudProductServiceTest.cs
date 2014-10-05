using System;
using System.Collections.Generic;
using System.Linq;
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
	public class CachingCloudProductServiceTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_ProductRepository_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).Returns(Enumerable.Empty<IProduct>());

			IProductService productService = CreateService(repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			repositoryMock.VerifyAll();
		}

		[TestMethod]
		public async Task Search_Should_Pass_ProductId_Of_Product_Returned_By_ProductRepository_Search_To_AzureProductProvider_Get()
		{
			// Arrange
			Guid productId = Guid.NewGuid();
			IProductRepository repository = CreateRepository(new[] { CreateProduct(productId) });

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(new ProductDescriptor());

			IProductService productService = CreateService(azureProvider: azureProviderMock.Object, repository: repository);

			// Act
			(await productService.Search("barcode", BarcodeType.EAN)).ToArray();

			// Assert
			azureProviderMock.VerifyAll();
		}

		[TestMethod]
		public async Task Search_Should_Return_ProductDescriptors_Returned_By_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Not_Empty_Sequence()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();
			Guid productId = Guid.NewGuid();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).Returns(new[] { CreateProduct(productId) });

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(expected);

			IProductService productService = CreateService(azureProvider: azureProviderMock.Object, repository: repositoryMock.Object);

			// Act
			var actual = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().BeEquivalentTo(new[] { expected });
		}

		[TestMethod]
		public async Task Search_Should_Not_Call_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Not_Empty_Sequence()
		{
			// Arrange
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).Returns(new[] { CreateProduct() });

			var awsProvider = new Mock<IAwsProductProvider>();

			IProductService productService = CreateService(awsProvider.Object, repository: repositoryMock.Object);

			// Act
			await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			awsProvider.Verify(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>()), Times.Never);
		}

		[TestMethod]
		public async Task Search_Should_Not_Call_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Empty_Sequence()
		{
			// Arrange
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).Returns(Enumerable.Empty<IProduct>);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(azureProvider: azureProviderMock.Object, repository: repositoryMock.Object);

			// Act
			await productService.Search("Barcode", BarcodeType.EAN);

			// Assert
			azureProviderMock.Verify(p => p.Get(It.IsAny<Guid>()), Times.Never);
		}

		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Empty_Sequence()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).Returns(Enumerable.Empty<IProduct>);

			IAwsProductProvider awsProvider = CreateAwsProvider();

			IProductService productService = CreateService(awsProvider, repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			Mock.Get(awsProvider).VerifyAll();
		}

		[TestMethod]
		public async Task Search_Should_Return_Null_When_AwsProvider_Search_Returns_Empty_Sequence()
		{
			var awsProviderMock = new Mock<IAwsProductProvider>();
			awsProviderMock.Setup(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new ProductDescriptor[0]);

			IProductService productService = CreateService(awsProviderMock.Object);

			// Act
			var descriptors = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			descriptors.Should().BeNull();
		}

		[TestMethod]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_ProductRepository_Save_When_ProductRepository_Search_Returns_Empty_Sequence()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).Returns(Enumerable.Empty<IProduct>());
			repositoryMock.Setup(r => r.Save(barcode, barcodeType)).ReturnsAsync(Guid.NewGuid());

			IProductService productService = CreateService(repository: repositoryMock.Object);

			// Act
			(await productService.Search(barcode, barcodeType)).ToArray();

			// Assert
			repositoryMock.VerifyAll();
		}

		[TestMethod]
		public async Task Search_Should_Pass_ProductDescriptor_Returned_By_AwsProductProvider_Search_To_AzureProductProvider_Save_When_ProductRepository_Search_ReturnsEmpty_Sequence()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			ProductDescriptor descriptor = new ProductDescriptor();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).Returns(Enumerable.Empty<IProduct>);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType)).ReturnsAsync(Guid.NewGuid());

			IAzureProductProvider azureProvider = CreateAzureProvider(descriptor);

			var awsProvider = new Mock<IAwsProductProvider>();
			awsProvider.Setup(p => p.Search(barcode, barcodeType)).ReturnsAsync(new[] { descriptor });

			IProductService productService = CreateService(awsProvider.Object, azureProvider, repositoryMock.Object);

			// Act
			(await productService.Search(barcode, barcodeType)).ToArray();

			// Assert
			Mock.Get(azureProvider).VerifyAll();
		}

		[TestMethod]
		public async Task Search_Should_Pass_Product_To_AzureProductProvider_Save_Having_ProductId_Returned_By_ProductRepository_Save_When_ProductRepository_Search_Returns_Empty_Sequence()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			Guid productId = Guid.NewGuid();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).Returns(Enumerable.Empty<IProduct>);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType)).ReturnsAsync(productId);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(azureProvider: azureProviderMock.Object, repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			azureProviderMock.VerifyAll();
		}

		[TestMethod]
		public async Task Search_Should_Return_ProductDescriptors_Returned_By_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Empty_Sequence()
		{
			// Arrange
			var expected = new[] { new ProductDescriptor(), new ProductDescriptor() };

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).Returns(Enumerable.Empty<IProduct>);

			var awsProvider = CreateAwsProvider(expected);

			IProductService productService = CreateService(awsProvider, repository: repositoryMock.Object);

			// Act
			var actual = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
		#endregion

		#region Get
		[TestMethod]
		public async Task Get_Should_Return_ProductDescriptor_Returned_By_AzureProductProvider_Get()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			Guid productId = Guid.NewGuid();
			IProductRepository repository = CreateRepository();

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(expected);

			IProductService productService = CreateService(azureProvider: azureProviderMock.Object, repository: repository);

			// Act
			ProductDescriptor actual = await productService.Get(productId);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IProductService CreateService(IAwsProductProvider awsProvider = null, IAzureProductProvider azureProvider = null, IProductRepository repository = null)
		{
			return new CachingCloudProductService(
				awsProvider ?? CreateAwsProvider(new[] { new ProductDescriptor() }),
				azureProvider ?? CreateAzureProvider(),
				repository ?? CreateRepository(Enumerable.Empty<IProduct>()));
		}

		private static IAwsProductProvider CreateAwsProvider(ProductDescriptor[] descriptors = null)
		{
			var awsProvider = new Mock<IAwsProductProvider>();
			awsProvider.Setup(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(descriptors ?? new[] { new ProductDescriptor() });
			return awsProvider.Object;
		}

		private static IAzureProductProvider CreateAzureProvider(ProductDescriptor descriptor = null)
		{
			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Save(descriptor ?? It.IsAny<ProductDescriptor>())).Returns(Task.Delay(0));
			return azureProviderMock.Object;
		}

		private static IProductRepository CreateRepository(IEnumerable<IProduct> products = null)
		{
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).Returns(products ?? new[] { CreateProduct() });
			return repositoryMock.Object;
		}

		private static IProduct CreateProduct(Guid? productId = null)
		{
			var productMock = new Mock<IProduct>();
			productMock.SetupGet(p => p.ProductId).Returns(productId ?? Guid.NewGuid());
			return productMock.Object;
		}
		#endregion
	}
}