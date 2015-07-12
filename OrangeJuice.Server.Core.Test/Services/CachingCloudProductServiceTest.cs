using System;
using System.Threading.Tasks;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Test.Services
{
	public class CachingCloudProductServiceTest
	{
		#region Search
		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_ProductRepository_Search(Type type)
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new Product[0]);

			IProductService productService = CreateService(type, repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			repositoryMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Pass_ProductId_Of_Product_Returned_By_ProductRepository_Search_To_AzureProductProvider_Get(Type type)
		{
			// Arrange
			Guid productId = Guid.NewGuid();
			IProductRepository repository = CreateRepository(new[] { CreateProduct(productId) });

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(new ProductDescriptor());

			IProductService productService = CreateService(type, azureProvider: azureProviderMock.Object, repository: repository);

			// Act
			await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			azureProviderMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Return_ProductDescriptors_Returned_By_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Not_Empty_Sequence(Type type)
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();
			Guid productId = Guid.NewGuid();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new[] { CreateProduct(productId) });

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(expected);

			IProductService productService = CreateService(type, azureProvider: azureProviderMock.Object, repository: repositoryMock.Object);

			// Act
			var actual = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().BeEquivalentTo(new[] { expected });
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Not_Call_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Not_Empty_Sequence(Type type)
		{
			// Arrange
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new[] { CreateProduct() });

			var awsProvider = new Mock<IAwsProductProvider>();

			IProductService productService = CreateService(type, awsProvider.Object, repository: repositoryMock.Object);

			// Act
			await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			awsProvider.Verify(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>()), Times.Never);
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Not_Call_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Empty_Sequence(Type type)
		{
			// Arrange
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new Product[0]);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(type, azureProvider: azureProviderMock.Object, repository: repositoryMock.Object);

			// Act
			await productService.Search("Barcode", BarcodeType.EAN);

			// Assert
			azureProviderMock.Verify(p => p.Get(It.IsAny<Guid>()), Times.Never);
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Pass_Barcode_BarcodeType_To_AwsProductProvider_Search_When_ProductRepository_Search_Returns_Empty_Sequence(Type type)
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new Product[0]);

			IAwsProductProvider awsProvider = CreateAwsProvider();

			IProductService productService = CreateService(type, awsProvider, repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			Mock.Get(awsProvider).VerifyAll();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Return_Null_When_ProductRepository_Returns_Empty_Sequence_And_AwsProvider_Search_Returns_Empty_Sequence(Type type)
		{
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new Product[0]);

			var awsProviderMock = new Mock<IAwsProductProvider>();
			awsProviderMock.Setup(p => p.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new ProductDescriptor[0]);

			IProductService productService = CreateService(type, awsProviderMock.Object, repository: repositoryMock.Object);

			// Act
			var descriptors = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			descriptors.Should().BeNull();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Pass_Barcode_BarcodeType_SourceProductId_To_ProductRepository_Save_When_ProductRepository_Search_Returns_Empty_Sequence(Type type)
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			const string sourceProductId = "ASIN";

			IAwsProductProvider awsProvider = CreateAwsProvider(new[] { new ProductDescriptor { SourceProductId = sourceProductId } });

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new Product[0]);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType, sourceProductId)).ReturnsAsync(Guid.NewGuid());

			IProductService productService = CreateService(type, awsProvider, repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			repositoryMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Pass_ProductDescriptor_Returned_By_AwsProductProvider_Search_To_AzureProductProvider_Save_When_ProductRepository_Search_ReturnsEmpty_Sequence(Type type)
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			const string sourceProductId = "ASIN";

			ProductDescriptor descriptor = new ProductDescriptor();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new Product[0]);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType, sourceProductId)).ReturnsAsync(Guid.NewGuid());

			IAzureProductProvider azureProvider = CreateAzureProvider(descriptor);

			var awsProvider = new Mock<IAwsProductProvider>();
			awsProvider.Setup(p => p.Search(barcode, barcodeType)).ReturnsAsync(new[] { descriptor });

			IProductService productService = CreateService(type, awsProvider.Object, azureProvider, repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			Mock.Get(azureProvider).VerifyAll();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Pass_Product_To_AzureProductProvider_Save_Having_ProductId_Returned_By_ProductRepository_Save_When_ProductRepository_Search_Returns_Empty_Sequence(Type type)
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			const string sourceProductId = "ASIN";

			Guid productId = Guid.NewGuid();

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(barcode, barcodeType)).ReturnsAsync(new Product[0]);
			repositoryMock.Setup(r => r.Save(barcode, barcodeType, sourceProductId)).ReturnsAsync(productId);

			var azureProviderMock = new Mock<IAzureProductProvider>();

			IProductService productService = CreateService(type, azureProvider: azureProviderMock.Object, repository: repositoryMock.Object);

			// Act
			await productService.Search(barcode, barcodeType);

			// Assert
			azureProviderMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Search_Should_Return_ProductDescriptors_Returned_By_AzureProductProvider_Get_When_ProductRepository_Search_Returns_Empty_Sequence(Type type)
		{
			// Arrange
			var expected = new[] { new ProductDescriptor(), new ProductDescriptor() };

			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(new Product[0]);

			var awsProvider = CreateAwsProvider(expected);

			IProductService productService = CreateService(type, awsProvider, repository: repositoryMock.Object);

			// Act
			var actual = await productService.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
		#endregion

		#region Get
		[Theory]
		[InlineData(typeof(CachingCloudProductService))]
		[InlineData(typeof(FSharp.Services.CachingCloudProductService))]
		public async Task Get_Should_Return_ProductDescriptor_Returned_By_AzureProductProvider_Get(Type type)
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			Guid productId = Guid.NewGuid();
			IProductRepository repository = CreateRepository();

			var azureProviderMock = new Mock<IAzureProductProvider>();
			azureProviderMock.Setup(p => p.Get(productId)).ReturnsAsync(expected);

			IProductService productService = CreateService(type, azureProvider: azureProviderMock.Object, repository: repository);

			// Act
			ProductDescriptor actual = await productService.Get(productId);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IProductService CreateService(Type type, IAwsProductProvider awsProvider = null, IAzureProductProvider azureProvider = null, IProductRepository repository = null)
		{
			return (IProductService)Activator.CreateInstance(type,
				awsProvider ?? CreateAwsProvider(new[] { new ProductDescriptor() }),
				azureProvider ?? CreateAzureProvider(),
				repository ?? CreateRepository());
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

		private static IProductRepository CreateRepository(Product[] products = null)
		{
			var repositoryMock = new Mock<IProductRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<BarcodeType>())).ReturnsAsync(products ?? new[] { CreateProduct() });
			return repositoryMock.Object;
		}

		private static Product CreateProduct(Guid? productId = null)
		{
			return new Product
			{
				ProductId = productId ?? Guid.NewGuid()
			};
		}
		#endregion
	}
}