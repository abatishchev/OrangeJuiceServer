using System;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlAwsProviderTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsClient_Is_Null()
		{
			// Arrange
			const IAwsClient client = null;

			// Act
			Action action = () => new XmlAwsProvider(client);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("client");
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
		private static IAwsProvider CreateClient(IQueryBuilder queryBuilder = null, IDocumentLoader documentLoader = null, IItemProvider itemProvider = null)
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
