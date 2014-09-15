using System;
using System.Collections.Generic;
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
			httpClientMock.Setup(c => c.GetStringAsync(url)).ReturnsAsync(String.Empty);

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
			const string xml = "";

			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStringAsync(It.IsAny<Uri>())).ReturnsAsync(xml);

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(xml)).Returns(Enumerable.Empty<XElement>());

			IAwsClient client = CreateClient(httpClient: httpClientMock.Object, itemSelector: selectorMock.Object);

			// Act
			await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			selectorMock.VerifyAll();
		}

		[TestMethod]
		public async Task GetItems_Should_Pass_Items_Returned_By_ItemSelector_SelectItems_To_ProductDescriptorFactory_Create()
		{
			// Arrange
			XElement[] elements = { new XElement("Item"), new XElement("Item") };

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<string>())).Returns(elements);

			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsIn(elements))).Returns(new ProductDescriptor());

			IAwsClient client = CreateClient(itemSelector: selectorMock.Object, factory: factoryMock.Object);

			// Act
			var items = (await client.GetItems(new ProductDescriptorSearchCriteria())).ToArray();

			// Assert
			factoryMock.VerifyAll();
		}

		[TestMethod]
		public async Task GetItems_Should_Return_ProductDescriptor_Returned_By_ProductDescriptorFactory_Create()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(expected);

			IAwsClient client = CreateClient(factory: factoryMock.Object);

			// Act
			var actual = await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			actual.Single().Should().Be(expected);
		}
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
				factory ?? CreateFactory());
		}

		private static IUrlBuilder CreateUrlBuilder(Uri url = null)
		{
			var builderMock = new Mock<IUrlBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<ProductDescriptorSearchCriteria>())).Returns(url ?? CreateUrl());
			return builderMock.Object;
		}

		private static IHttpClient CreateHttpClient(string xml = "")
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(l => l.GetStringAsync(It.IsAny<Uri>())).ReturnsAsync(xml);
			return httpClientMock.Object;
		}

		private static IItemSelector CreateItemSelector(IEnumerable<XElement> elements = null)
		{
			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<string>())).Returns(elements ?? new[] { new XElement("Item") });
			return selectorMock.Object;
		}

		private static IFactory<XElement, ProductDescriptor> CreateFactory(ProductDescriptor descriptor = null)
		{
			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(descriptor ?? new ProductDescriptor());
			return factoryMock.Object;
		}
		#endregion
	}
}