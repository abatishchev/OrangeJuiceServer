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
		public void Ctor_Should_Throw_Exception_When_UrBuilder_Is_Null()
		{
			// Arrange
			const IQueryBuilder urlBuilder = null;
			const IDocumentLoader documentLoader = null;
			const IItemProvider itemProvider = null;

			// Act
			Action action = () => new XmlAwsClient(urlBuilder, documentLoader, itemProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("argumentBuilder");
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
			var agumentBuilderMock = CreateUrlBuilder(callback);

			IAwsClient client = CreateClient(agumentBuilderMock.Object);

			// Act
			await client.SearchItem(title);

			// Assert
			agumentBuilderMock.Verify(b => b.BuildUrl(It.IsAny<StringDictionary>()), Times.Once);
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

		[TestMethod]
		public void LookupAttributes_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
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

		[TestMethod]
		public void LookupImages_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
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

		private static IQueryBuilder CreateUrlBuilder()
		{
			return CreateUrlBuilder(null).Object;
		}

		private static Mock<IQueryBuilder> CreateUrlBuilder(Action<StringDictionary> callback)
		{
			var builderMock = new Mock<IQueryBuilder>();
			builderMock.Setup(b => b.BuildUrl(It.IsAny<StringDictionary>()))
					   .Returns("query")
					   .Callback<StringDictionary>(d =>
					   {
						   if (callback != null)
							   callback(d);
					   });
			return builderMock;
		}

		private static IDocumentLoader CreateDocumentLoader()
		{
			var loaderMock = new Mock<IDocumentLoader>();
			loaderMock.Setup(l => l.Load(It.IsAny<string>()))
					  .ReturnsAsync(XDocument.Parse(
@"<?xml version='1.0'?>
<Items xmlns=''>
</Items>"));
			return loaderMock.Object;
		}

		private static IItemProvider CreateItemProvider()
		{
			var providerMock = new Mock<IItemProvider>();
			return providerMock.Object;
		}
		#endregion
	}
}