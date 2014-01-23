using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsClientTest
	{
		#region SelectItems
		[TestMethod]
		public async Task GetItems_Should_Pass_Query_Returned_By_QueryBuilder_To_DocumentLoader_Load()
		{
			// Arrange
			Uri url = CreateUrl();

			var builderMock = CreateUrlBuilder(url);

			var loaderMock = new Mock<IDocumentLoader>();
			loaderMock.Setup(l => l.Load(url)).ReturnsAsync(new XDocument());

			IAwsClient client = CreateClient(builderMock, loaderMock.Object);

			// Act
			await client.GetItems(new StringDictionary());

			// Assert
			loaderMock.Verify(l => l.Load(url), Times.Once());
		}

		[TestMethod]
		public async Task GetItems_Should_Pass_Document_Returned_By_DocumentLoader_To_ItemSelector_GetItems()
		{
			// Arrange
			XDocument doc = new XDocument();

			var loaderMock = new Mock<IDocumentLoader>();
			loaderMock.Setup(l => l.Load(It.IsAny<Uri>())).ReturnsAsync(doc);

			var selectorMock = new Mock<IItemSelector>();

			IAwsClient client = CreateClient(documentLoader: loaderMock.Object, itemSelector: selectorMock.Object);

			// Act
			await client.GetItems(new StringDictionary());

			// Assert
			selectorMock.Verify(s => s.SelectItems(doc), Times.Once());
		}

		[TestMethod]
		public async Task GetItems_Should_Return_Elements_Returned_By_ItemSelector_GetItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var selectorMock = CreateItemSelector(expected);

			IAwsClient client = CreateClient(itemSelector: selectorMock);

			// Act
			var actual = await client.GetItems(new StringDictionary());

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static Uri CreateUrl()
		{
			return new Uri("http://example.com");
		}

		private static IAwsClient CreateClient(IQueryBuilder queryBuilder = null, IDocumentLoader documentLoader = null, IItemSelector itemSelector = null)
		{
			return new AwsClient(
				queryBuilder ?? CreateUrlBuilder(),
				documentLoader ?? CreateDocumentLoader(),
				itemSelector ?? CreateItemSelector());
		}

		private static IQueryBuilder CreateUrlBuilder(Uri url = null)
		{
			var builderMock = new Mock<IQueryBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<IStringDictionary>())).Returns(url ?? CreateUrl());
			return builderMock.Object;
		}

		private static IDocumentLoader CreateDocumentLoader(XDocument doc = null)
		{
			var loaderMock = new Mock<IDocumentLoader>();
			loaderMock.Setup(l => l.Load(It.IsAny<Uri>())).ReturnsAsync(doc ?? new XDocument());
			return loaderMock.Object;
		}

		private static IItemSelector CreateItemSelector(IEnumerable<XElement> elements = null)
		{
			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.SelectItems(It.IsAny<XDocument>())).Returns(elements ?? new[] { new XElement("Item") });
			return selectorMock.Object;
		}
		#endregion
	}
}