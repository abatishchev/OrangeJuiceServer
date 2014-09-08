using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlAwsClientTest
	{
		#region SelectItems
		[TestMethod]
		public async Task GetItems_Should_Pass_Query_Returned_By_QueryBuilder_To_HttpClient_GetStreamAsync()
		{
			// Arrange
			Uri url = CreateUrl();

			var builderMock = CreateUrlBuilder(url);

			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStreamAsync(url)).ReturnsAsync(new MemoryStream());

			IAwsClient client = CreateClient(builderMock, httpClientMock.Object);

			// Act
			await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			httpClientMock.VerifyAll();
		}

		[TestMethod]
		public async Task GetItems_Should_Pass_Stream_Returned_By_HttpClient_To_ItemSelector_SelectItems()
		{
			// Arrange
			Stream stream = new MemoryStream();

			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStreamAsync(It.IsAny<Uri>())).ReturnsAsync(stream);

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(stream)).Returns(Enumerable.Empty<XElement>());

			IAwsClient client = CreateClient(httpClient: httpClientMock.Object, itemSelector: selectorMock.Object);

			// Act
			await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			selectorMock.VerifyAll();
		}

		[TestMethod]
		public async Task GetItems_Should_Return_Elements_Returned_By_ItemSelector_SelectItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var selectorMock = CreateItemSelector(expected);

			IAwsClient client = CreateClient(itemSelector: selectorMock);

			// Act
			var actual = await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

/*
		[TestMethod]
		public async Task GetItems_Should_Pass_First_Item_To_ProductDescriptorFactory_Create()
		{
			// Arrange
			XElement[] elements = { new XElement("Item"), new XElement("Item") };

			IAwsClient client = CreateClient(elements);

			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsIn(elements))).Returns(new ProductDescriptor());

			IAwsProductProvider provider = CreateProvider(client, factoryMock.Object);

			// Act
			await provider.Search("barcode", BarcodeType.EAN);

			// Assert
			factoryMock.Verify(f => f.Create(elements.First()), Times.Once);
		}

		[TestMethod]
		public async Task GetItems_Should_Return_Descriptor_Returned_By_ProductDescriptorFactory_Create()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			IAwsClient client = CreateClient();

			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(expected);

			IAwsProductProvider provider = CreateProvider(client, factoryMock.Object);

			// Act
			ProductDescriptor actual = await provider.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().Be(expected);
		}
*/
		#endregion

		#region Helper methods
		private static Uri CreateUrl()
		{
			return new Uri("http://example.com");
		}

		private static IAwsClient CreateClient(IUrlBuilder urlBuilder = null, IHttpClient httpClient = null, IItemSelector itemSelector = null, IFactory<XElement, ProductDescriptor> factory = null)
		{
			return new XmlAwsClient(
				urlBuilder ?? CreateUrlBuilder(),
				httpClient ?? CreateHttpClient(),
				itemSelector ?? CreateItemSelector(),
				factory);
		}

		private static IUrlBuilder CreateUrlBuilder(Uri url = null)
		{
			var builderMock = new Mock<IUrlBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<ProductDescriptorSearchCriteria>())).Returns(url ?? CreateUrl());
			return builderMock.Object;
		}

		private static IHttpClient CreateHttpClient(Stream stream = null)
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(l => l.GetStreamAsync(It.IsAny<Uri>())).ReturnsAsync(stream ?? new MemoryStream());
			return httpClientMock.Object;
		}

		private static IItemSelector CreateItemSelector(IEnumerable<XElement> elements = null)
		{
			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<Stream>())).Returns(elements ?? new[] { new XElement("Item") });
			return selectorMock.Object;
		}
		#endregion
	}
}