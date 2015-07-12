using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Factory;
using FluentAssertions;
using Moq;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

using Xunit;

namespace OrangeJuice.Server.Test.Services
{
	public class XmlAwsClientTest
	{
		#region GetItems
		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Pass_Query_Returned_By_QueryBuilder_To_HttpClient_GetStreamAsync(Type type)
		{
			// Arrange
			Uri url = CreateUrl();

			var builderMock = CreateUrlBuilder(url);

			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStringAsync(url)).ReturnsAsync(String.Empty);

			IAwsClient client = CreateClient(type, builderMock, httpClientMock.Object);

			// Act
			await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			httpClientMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Pass_Stream_Returned_By_HttpClient_To_ItemSelector_SelectItems(Type type)
		{
			// Arrange
			const string xml = "";

			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStringAsync(It.IsAny<Uri>())).ReturnsAsync(xml);

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(xml)).Returns(Enumerable.Empty<XElement>());

			IAwsClient client = CreateClient(type, httpClient: httpClientMock.Object, itemSelector: selectorMock.Object);

			// Act
			await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			selectorMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Pass_Items_Returned_By_ItemSelector_SelectItems_To_ProductDescriptorFactory_Create(Type type)
		{
			// Arrange
			XElement[] elements = { new XElement("Item"), new XElement("Item") };

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<string>())).Returns(elements);

			var factoryMock = new Mock<IFactory<ProductDescriptor, XElement>>();
			factoryMock.Setup(f => f.Create(It.IsIn(elements))).Returns(new ProductDescriptor());

			IAwsClient client = CreateClient(type, itemSelector: selectorMock.Object, factory: factoryMock.Object);

			// Act
			await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			factoryMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Return_ProductDescriptor_Returned_By_ProductDescriptorFactory_Create(Type type)
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			var factoryMock = new Mock<IFactory<ProductDescriptor, XElement>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(expected);

			IAwsClient client = CreateClient(type, factory: factoryMock.Object);

			// Act
			ProductDescriptor[] actual = await client.GetItems(new ProductDescriptorSearchCriteria());

			// Assert
			actual.Single().Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static Uri CreateUrl()
		{
			return new Uri("http://example.com");
		}

		private static IAwsClient CreateClient(Type type, IUrlBuilder urlBuilder = null, IHttpClient httpClient = null, IItemSelector itemSelector = null, IFactory<ProductDescriptor, XElement> factory = null)
		{
			return (IAwsClient)Activator.CreateInstance(type,
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

		private static IHttpClient CreateHttpClient(string content = "")
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStringAsync(It.IsAny<Uri>())).ReturnsAsync(content);
			return httpClientMock.Object;
		}

		private static IItemSelector CreateItemSelector(IEnumerable<XElement> elements = null)
		{
			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<string>())).Returns(elements ?? new[] { new XElement("Item") });
			return selectorMock.Object;
		}

		private static IFactory<ProductDescriptor, XElement> CreateFactory(ProductDescriptor descriptor = null)
		{
			var factoryMock = new Mock<IFactory<ProductDescriptor, XElement>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(descriptor ?? new ProductDescriptor());
			return factoryMock.Object;
		}
		#endregion
	}
}