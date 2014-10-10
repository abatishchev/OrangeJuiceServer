using System;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	public class AzureProductProviderTest
	{
		#region Get
		[Fact]
		public async Task Get_Should_Pass_AzureOptions_ProductContainer_And_ProductId_To_AzureClient_GetBlobFromContainer()
		{
			// Arrange
			const string containerName = "containerName";
			AzureOptions azureOptions = new AzureOptions { ProductsContainer = containerName };
			Guid productId = Guid.NewGuid();

			IAzureClient client = CreateClient(containerName, productId.ToString());

			IAzureProductProvider provider = CreateProvider(azureOptions, client);

			// Act
			await provider.Get(productId);

			// Assert
			Mock.Get(client).Verify(c => c.GetBlobFromContainer(containerName, productId.ToString()), Times.Once);
		}

		[Fact]
		public async Task Get_Should_Pass_Content_Returned_By_AzureClient_GetBlobFromContainer_To_Converter_ConvertBack()
		{
			// Arrange
			const string content = "content";
			AzureOptions azureOptions = new AzureOptions { ProductsContainer = "containerName" };

			IAzureClient client = CreateClient(content);

			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.ConvertBack(It.IsAny<ProductDescriptor>())).Returns(content);

			IAzureProductProvider provider = CreateProvider(azureOptions, client, converterMock.Object);

			// Act
			await provider.Get(Guid.NewGuid());

			// Assert
			converterMock.Verify(c => c.Convert(content), Times.Once);
		}

		[Fact]
		public async Task Get_Should_Return_ProductDescriptor_Returned_By_Converter()
		{
			// Arrange
			const string content = "content";
			ProductDescriptor expected = new ProductDescriptor();

			IAzureClient client = CreateClient(content);

			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.Convert(content)).Returns(expected);

			IAzureProductProvider provider = CreateProvider(client, converterMock.Object);

			// Act
			ProductDescriptor actual = await provider.Get(Guid.NewGuid());

			// Arrange
			actual.Should().Be(expected);
		}

		[Fact]
		public async Task Get_Should_Return_Null_When_AzureClient_GetBlobFromContainer_Returns_Null()
		{
			// Arrange
			var clientMock = new Mock<IAzureClient>();
			clientMock.Setup(c => c.GetBlobFromContainer(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(null);

			IAzureProductProvider provider = CreateProvider(clientMock.Object);

			// Act
			ProductDescriptor actual = await provider.Get(Guid.NewGuid());

			// Arrange
			actual.Should().BeNull();
		}
		#endregion

		#region Save
		[Fact]
		public async Task Save_Should_Pass_ProductDescriptor_To_Converter_ConvertBack()
		{
			// Arrange
			ProductDescriptor descriptor = new ProductDescriptor();

			var converter = CreateConverter();

			IAzureProductProvider provider = CreateProvider(converter: converter);

			// Act
			await provider.Save(descriptor);

			// Assert
			Mock.Get(converter).Verify(c => c.ConvertBack(descriptor), Times.Once);
		}

		[Fact]
		public async Task Set_Should_Pass_Content_Returned_By_Converter_ConvertBack_To_AzureClient_PutBlobToContainer()
		{
			// Arrange
			ProductDescriptor descriptor = new ProductDescriptor();
			const string content = "content";

			IAzureClient client = CreateClient(content);

			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.ConvertBack(descriptor)).Returns(content);

			IAzureProductProvider provider = CreateProvider(client, converterMock.Object);

			// Act
			await provider.Save(descriptor);

			// Arrange
			Mock.Get(client).Verify(c => c.PutBlobToContainer(It.IsAny<string>(), It.IsAny<string>(), content), Times.Once);
		}

		[Fact]
		public async Task Set_Should_Pass_AzureOptions_ProductContainer_ProductId_And_ProductDescriptor_ProductId_To_AzureClient_PutBlobToContainer()
		{
			// Arrange
			const string containerName = "containerName";
			AzureOptions azureOptions = new AzureOptions { ProductsContainer = containerName };

			Guid productId = Guid.NewGuid();
			ProductDescriptor descriptor = new ProductDescriptor { ProductId = productId };

			IAzureClient client = CreateClient(containerName, productId.ToString());

			IAzureProductProvider provider = CreateProvider(azureOptions, client);

			// Act
			await provider.Save(descriptor);

			// Arrange
			Mock.Get(client).Verify(c => c.PutBlobToContainer(containerName, productId.ToString(), It.IsAny<string>()), Times.Once);
		}
		#endregion

		#region Helper methods

		private static IAzureProductProvider CreateProvider(IAzureClient client, IConverter<string, ProductDescriptor> converter = null)
		{
			return CreateProvider(new AzureOptions(), client, converter);
		}

		private static IAzureProductProvider CreateProvider(AzureOptions azureOptions = null, IAzureClient client = null, IConverter<string, ProductDescriptor> converter = null)
		{
			return new AzureProductProvider(
				azureOptions ?? new AzureOptions(),
				client ?? CreateClient(null),
				converter ?? CreateConverter());
		}

		private static IAzureClient CreateClient(string containerName, string fileName)
		{
			var clientMock = new Mock<IAzureClient>();
			clientMock.Setup(c => c.GetBlobFromContainer(containerName, fileName)).ReturnsAsync("content");
			clientMock.Setup(c => c.PutBlobToContainer(containerName, fileName, It.IsAny<string>())).Returns(Task.Delay(0));
			return clientMock.Object;
		}

		private static IAzureClient CreateClient(string content)
		{
			var clientMock = new Mock<IAzureClient>();
			clientMock.Setup(c => c.GetBlobFromContainer(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(content);
			clientMock.Setup(c => c.PutBlobToContainer(It.IsAny<string>(), It.IsAny<string>(), content)).Returns(Task.Delay(0));
			return clientMock.Object;
		}

		private static IConverter<string, ProductDescriptor> CreateConverter()
		{
			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.Convert(It.IsAny<string>())).Returns(new ProductDescriptor());
			converterMock.Setup(c => c.ConvertBack(It.IsAny<ProductDescriptor>())).Returns("content");
			return converterMock.Object;
		}
		#endregion
	}
}