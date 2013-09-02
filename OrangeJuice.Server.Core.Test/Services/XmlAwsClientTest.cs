using System;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

using StringDictionary = System.Collections.Generic.IDictionary<string, string>;

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

		#region SearchItem
		[TestMethod]
		public void SearchItem_Should_Throw_Exception_When_Title_Is_Null()
		{
			// Arrange
			const string title = null;

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.SearchItem(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public void SearchItem_Should_Throw_Exception_When_Title_Is_Empty()
		{
			// Arrange
			const string title = "";

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.SearchItem(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public async Task SearchItem_Should_Pass_Arguments_To_UrlBuilder()
		{
			// Arrange
			const string title = "anyTitle";

			Action<StringDictionary> callback = d => d.Should()
													  .Contain("Operation", "ItemSearch")
													  .And.Contain("SearchIndex", "Grocery")
													  .And.Contain("ResponseGroup", "Small")
													  .And.Contain("Title", title);
			var builderMock = new Mock<IQueryBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<StringDictionary>())).Returns("query").Callback(callback);

			IAwsClient client = CreateClient(builderMock.Object);

			// Act
			await client.SearchItem(title);

			// Assert
			builderMock.Verify(b => b.BuildUrl(It.IsAny<StringDictionary>()), Times.Once());
		}

		[TestMethod]
		public async Task SearchItem_Should_Pass_Query_Returned_By_QueryBuilder_To_DocumentLoader_Load()
		{
			// Arrange
			const string url = "anyUrl";

			var builderMock = CreateUrlBuilder(url);

			var loaderMock = new Mock<IDocumentLoader>();
			loaderMock.Setup(l => l.Load(url)).ReturnsAsync(new XDocument());

			IAwsClient client = CreateClient(builderMock, loaderMock.Object);
			const string title = "anyTitle";

			// Act
			await client.SearchItem(title);

			// Assert
			loaderMock.Verify(l => l.Load(url), Times.Once());
		}

		[TestMethod]
		public async Task SearchItem_Should_Pass_Document_Returned_By_DocumentLoader_To_ItemProvider_GetItems()
		{
			// Arrange
			XDocument doc = new XDocument();

			var loaderMock = CreateDocumentLoader(doc);

			var providerMock = new Mock<IItemProvider>();
			providerMock.Setup(l => l.GetItems(doc)).Returns(new XElement("Item"));

			IAwsClient client = CreateClient(documentLoader: loaderMock, itemProvider: providerMock.Object);
			const string title = "anyTitle";

			// Act
			await client.SearchItem(title);

			// Assert
			providerMock.Verify(p => p.GetItems(doc), Times.Once());
		}
		#endregion

		#region LookupAttributes
		[TestMethod]
		public void LookupAttributes_Should_Throw_Exception_When_Id_Is_Null()
		{
			// Arrange
			const string id = null;

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.LookupAttributes(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}

		[TestMethod]
		public void LookupAttributes_Should_Throw_Exception_When_Id_Is_Empty()
		{
			// Arrange
			const string id = "";

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.LookupAttributes(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}
		#endregion

		#region LookupImages
		[TestMethod]
		public void LookupImages_Should_Throw_Exception_When_Id_Is_Null()
		{
			// Arrange
			const string id = null;

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.LookupImages(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}

		[TestMethod]
		public void LookupImages_Should_Throw_Exception_When_Id_Is_Empty()
		{
			// Arrange
			const string id = "";

			IAwsClient client = CreateClient();

			// Act
			Func<Task> action = () => client.LookupImages(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
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
			builderMock.Setup(b => b.BuildUrl(It.IsAny<StringDictionary>())).Returns(query ?? "query");
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