using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Filters;
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
			await client.GetItems(new AwsProductSearchCriteria());

			// Assert
			httpClientMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Pass_String_Returned_By_HttpClient_To_ItemSelector_SelectItems(Type type)
		{
			// Arrange
			const string xml = "";

			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(c => c.GetStringAsync(It.IsAny<Uri>())).ReturnsAsync(xml);

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(xml)).Returns(Enumerable.Empty<XElement>());

			IAwsClient client = CreateClient(type, httpClient: httpClientMock.Object, itemSelector: selectorMock.Object);

			// Act
			await client.GetItems(new AwsProductSearchCriteria());

			// Assert
			selectorMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Return_XElement_Returned_By_ItemSelector_SelectItems_And_Filtered_By_ItemsFilter_Filter(Type type)
		{
			// Arrange
			var expected = new[] { new XElement("ItemA"), new XElement("ItemB"), new XElement("ItemC") };

			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<string>())).Returns(expected);

			var filterMock = new Mock<IFilter<XElement>>();
			filterMock.Setup(s => s.Filter(It.IsAny<XElement>())).Returns<XElement>(e => e.Name == "ItemA");

			IAwsClient client = CreateClient(type, itemSelector: selectorMock.Object);

			// Act
			var actual = await client.GetItems(new AwsProductSearchCriteria());

			// Assert
			actual.Should().BeEquivalentTo((IEnumerable)expected);
		}
		#endregion

		#region Helper methods
		private static Uri CreateUrl()
		{
			return new Uri("http://example.com");
		}

		private static IAwsClient CreateClient(Type type,
											   IUrlBuilder urlBuilder = null,
											   IHttpClient httpClient = null,
											   IItemSelector itemSelector = null,
											   IFilter<XElement> itemFilter = null)
		{
			return (IAwsClient)Activator.CreateInstance(type,
				urlBuilder ?? CreateUrlBuilder(),
				httpClient ?? CreateHttpClient(),
				itemSelector ?? CreateItemSelector(),
				itemFilter ?? CreateFilter());
		}

		private static IUrlBuilder CreateUrlBuilder(Uri url = null)
		{
			var builderMock = new Mock<IUrlBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<AwsProductSearchCriteria>())).Returns(url ?? CreateUrl());
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

		private static IFilter<XElement> CreateFilter(bool value = true)
		{
			var filterMock = new Mock<IFilter<XElement>>();
			filterMock.Setup(s => s.Filter(It.IsAny<XElement>())).Returns(value);
			return filterMock.Object;
		}
		#endregion
	}
}