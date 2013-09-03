using System;
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
	public class XmlAwsClientTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_QueryBuilder_Is_Null()
		{
			// Arrange
			const IQueryBuilder queryBuilder = null;
			const IDocumentLoader documentLoader = null;
			const IItemProvider itemProvider = null;

			// Act
			Action action = () => new XmlAwsClient(queryBuilder, documentLoader, itemProvider);

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
			const IItemProvider itemProvider = null;

			// Act
			Action action = () => new XmlAwsClient(queryBuilder, documentLoader, itemProvider);

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
			const IItemProvider itemProvider = null;

			// Act
			Action action = () => new XmlAwsClient(queryBuilder, documentLoader, itemProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("itemProvider");
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
			var args = new StringDictionary();

			// Act
			await client.GetItems(args);

			// Assert
			loaderMock.Verify(l => l.Load(url), Times.Once());
		}

		[TestMethod]
		public async Task GetItems_Should_Pass_Document_Returned_By_DocumentLoader_To_ItemProvider_GetItems()
		{
			// Arrange
			XElement expected = new XElement("Items");

			var providerMock = new Mock<IItemProvider>();
			providerMock.Setup(p => p.GetItems(It.IsAny<XDocument>())).Returns(expected);

			IAwsClient client = CreateClient(itemProvider: providerMock.Object);
			var args = new StringDictionary();

			// Act
			XElement actual = await client.GetItems(args);

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetItems_Should_Return_Element_Returned_By_Item_Provider()
		{
			Assert.Inconclusive("TODO");
		}
		#endregion

		#region Helper methods
		private static IAwsClient CreateClient(IQueryBuilder queryBuilder = null, IDocumentLoader documentLoader = null, IItemProvider itemProvider = null)
		{
			return new XmlAwsClient(
				queryBuilder ?? CreateUrlBuilder(),
				documentLoader ?? CreateDocumentLoader(),
				itemProvider ?? CreateItemProvider());
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

		private static IItemProvider CreateItemProvider(XElement element = null)
		{
			var providerMock = new Mock<IItemProvider>();
			providerMock.Setup(p => p.GetItems(It.IsAny<XDocument>())).Returns(element ?? new XElement("Item"));
			return providerMock.Object;
		}
		#endregion
	}
}