using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AzureProductProviderTest
	{
		#region Get
		[TestMethod]
		public async Task Get_Should_Pass_AzureOptions_ProductContainer_And_ProductId_To_AzureClient_GetBlobFromContainer()
		{
			// Arrange
			const string containerName = "containerName";
			AzureOptions azureOptions = new AzureOptions { ProductContainer = containerName };
			Guid productId = Guid.NewGuid();

			var clientMock = CreateClient(containerName, productId.ToString());

			IAzureProductProvider provider = CreateProvider(azureOptions, clientMock.Object);

			// Act
			await provider.Get(productId);

			// Assert
			clientMock.Verify(c => c.GetBlobFromContainer(containerName, productId.ToString()), Times.Once);
		}

		[TestMethod]
		public async Task Get_Should_Pass_Content_Returned_By_AzureClient_GetBlobFromContainer_To_Converter_Convert()
		{
			// Arrange
			const string content = "content";
			const string containerName = "containerName";
			AzureOptions azureOptions = new AzureOptions { ProductContainer = containerName };
			Guid productId = Guid.NewGuid();

			var clientMock = CreateClient(containerName, productId.ToString(), content);

			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.Convert(It.IsAny<ProductDescriptor>())).Returns(content);

			IAzureProductProvider provider = CreateProvider(azureOptions, clientMock.Object, converterMock.Object);

			// Act
			await provider.Get(productId);

			// Assert
			converterMock.Verify(c => c.Convert(content), Times.Once);
		}

		[TestMethod]
		public async Task Get_Should_Return_ProductDescriptor_Returned_By_Converter()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();
			const string content = "content";

			var clientMock = new Mock<IAzureClient>();
			clientMock.Setup(c => c.GetBlobFromContainer(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(content);

			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.Convert(content)).Returns(expected);

			IAzureProductProvider provider = CreateProvider(clientMock.Object, converterMock.Object);

			// Act
			ProductDescriptor actual = await provider.Get(Guid.NewGuid());

			// Arrange
			actual.Should().Be(expected);
		}

		[TestMethod]
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
		[TestMethod]
		public async Task Save_Should_Pass_ProductDescriptor_To_Converter_Convert()
		{
			// Arrange
			ProductDescriptor descriptor = new ProductDescriptor();

			var converterMock = CreateConverter();

			IAzureProductProvider provider = CreateProvider(converter: converterMock.Object);

			// Act
			await provider.Save(descriptor);

			// Assert
			converterMock.Verify(c => c.Convert(descriptor), Times.Once);
		}

		[TestMethod]
		public async Task Set_Should_Pass_Content_Returned_By_Converter_Convert_To_AzureClient_PutBlobToContainer()
		{
			// Arrange
			ProductDescriptor descriptor = new ProductDescriptor();
			const string content = "content";

			var clientMock = new Mock<IAzureClient>();
			clientMock.Setup(c => c.GetBlobFromContainer(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(content);

			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.Convert(content)).Returns(descriptor);

			IAzureProductProvider provider = CreateProvider(clientMock.Object, converterMock.Object);

			// Act
			await provider.Save(descriptor);

			// Arrange
			clientMock.Verify(c => c.PutBlobToContainer(It.IsAny<string>(), It.IsAny<string>(), content), Times.Once);
		}

		[TestMethod]
		public async Task Set_Should_Pass_AzureOptions_ProductContainer_ProductId_And_ProductDescriptor_ProductId_To_AzureClient_PutBlobToContainer()
		{
			// Arrange
			const string containerName = "containerName";
			AzureOptions azureOptions = new AzureOptions { ProductContainer = containerName };

			Guid productId = Guid.NewGuid();
			ProductDescriptor descriptor = new ProductDescriptor { ProductId = productId };

			var clientMock = CreateClient(containerName, productId.ToString());

			IAzureProductProvider provider = CreateProvider(azureOptions, clientMock.Object);

			// Act
			await provider.Save(descriptor);

			// Arrange
			clientMock.Verify(c => c.PutBlobToContainer(containerName, productId.ToString(), It.IsAny<string>()), Times.Once);
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
				client ?? CreateClient().Object,
				converter ?? CreateConverter().Object);
		}

		private static Mock<IAzureClient> CreateClient(string containerName = null, string fileName = null, string content = null)
		{
			var clientMock = new Mock<IAzureClient>();
			clientMock.Setup(c => c.GetBlobFromContainer(containerName ?? It.IsAny<string>(), fileName ?? It.IsAny<string>())).ReturnsAsync(content ?? "content");
			clientMock.Setup(c => c.PutBlobToContainer(containerName ?? It.IsAny<string>(), fileName ?? It.IsAny<string>(), content ?? It.IsAny<string>())).Returns(Task.Delay(0));
			return clientMock;
		}

		private static Mock<IConverter<string, ProductDescriptor>> CreateConverter(ProductDescriptor descriptor = null, string content = null)
		{
			var converterMock = new Mock<IConverter<string, ProductDescriptor>>();
			converterMock.Setup(c => c.Convert(It.IsAny<string>())).Returns(descriptor ?? new ProductDescriptor());
			converterMock.Setup(c => c.Convert(It.IsAny<ProductDescriptor>())).Returns(content ?? "content");
			return converterMock;
		}
		#endregion
	}
}