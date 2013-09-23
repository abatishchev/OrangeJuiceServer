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
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_QueryBuilder_Is_Null()
		{
			// Arrange
			const IQueryBuilder queryBuilder = null;
			const IDocumentLoader documentLoader = null;
			const IItemSelector itemProvider = null;

			// Act
			Action action = () => new AwsClient(queryBuilder, documentLoader, itemProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("queryBuilder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_DocumentLoader_Is_Null()
		{
			// Arrange
			IQueryBuilder queryBuilder = CreateUrlBuilder();
			const IDocumentLoader documentLoader = null;
			const IItemSelector itemProvider = null;

			// Act
			Action action = () => new AwsClient(queryBuilder, documentLoader, itemProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("documentLoader");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ItemProvider_Is_Null()
		{
			// Arrange
			IQueryBuilder queryBuilder = CreateUrlBuilder();
			IDocumentLoader documentLoader = CreateDocumentLoader();
			const IItemSelector itemProvider = null;

			// Act
			Action action = () => new AwsClient(queryBuilder, documentLoader, itemProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("itemSelector");
		}
		#endregion

		#region GetItems
		[TestMethod]
		public void GetItems_Should_Throw_Exception_When_Args_Is_Null()
		{
			// Arrange
			const IStringDictionary args = null;

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.GetItems(args);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("args");
		}

		[TestMethod]
		public async Task GetItems_Should_Pass_Query_Returned_By_QueryBuilder_To_DocumentLoader_Load()
		{
			// Arrange
			const string url = "anyUrl";

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
			loaderMock.Setup(l => l.Load(It.IsAny<string>())).ReturnsAsync(doc);

			var selectorMock = new Mock<IItemSelector>();

			IAwsClient client = CreateClient(documentLoader: loaderMock.Object, itemSelector: selectorMock.Object);

			// Act
			await client.GetItems(new StringDictionary());

			// Assert
			selectorMock.Verify(s => s.GetItems(doc), Times.Once());
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
		private static IAwsClient CreateClient(IQueryBuilder queryBuilder = null, IDocumentLoader documentLoader = null, IItemSelector itemSelector = null)
		{
			return new AwsClient(
				queryBuilder ?? CreateUrlBuilder(),
				documentLoader ?? CreateDocumentLoader(),
				itemSelector ?? CreateItemSelector());
		}

		private static IQueryBuilder CreateUrlBuilder(string query = null)
		{
			var builderMock = new Mock<IQueryBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<IStringDictionary>())).Returns(query ?? "query");
			return builderMock.Object;
		}

		private static IDocumentLoader CreateDocumentLoader(XDocument doc = null)
		{
			var loaderMock = new Mock<IDocumentLoader>();
			loaderMock.Setup(l => l.Load(It.IsAny<string>())).ReturnsAsync(doc ?? new XDocument());
			return loaderMock.Object;
		}

		private static IItemSelector CreateItemSelector(IEnumerable<XElement> elements = null)
		{
			var selectorMock = new Mock<IItemSelector>();
			selectorMock.Setup(s => s.GetItems(It.IsAny<XDocument>())).Returns(elements ?? new[] { new XElement("Item") });
			return selectorMock.Object;
		}
		#endregion
	}
}