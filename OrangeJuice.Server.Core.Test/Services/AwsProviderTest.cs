using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsProviderTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Client_Is_Null()
		{
			// Arrange
			const IAwsClient client = null;

			// Act
			Action action = () => new AwsProvider(client);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("client");
		}
		#endregion

		#region SearchItems
		[TestMethod]
		public void SearchItems_Should_Throw_Exception_When_Title_Is_Null()
		{
			// Arrange
			const string title = null;

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.SearchItems(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public void SearchItems_Should_Throw_Exception_When_Title_Is_Empty()
		{
			// Arrange
			const string title = "";

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.SearchItems(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public async Task SearchItems_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			const string title = "anyTitle";

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemSearch")
													   .And.Contain("SearchIndex", "Grocery")
													   .And.Contain("ResponseGroup", "Small")
													   .And.Contain("Title", title);
			var clientMock = CreateClient(callback: callback);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.SearchItems(title);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once());
		}

		[TestMethod]
		public async Task SearchItems_Should_Return_Elements_Returned_By_Client_GetItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var clientMock = CreateClient(expected);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			var actual = await provider.SearchItems("anyTitle");

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region LookupAttributes
		[TestMethod]
		public void LookupAttributes_Should_Throw_Exception_When_Ids_Is_Null()
		{
			// Arrange
			const string[] ids = null;

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.LookupAttributes(ids);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("ids");
		}

		[TestMethod]
		public async Task LookupAttributes_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			string[] ids = new[] { "id1", "id2" };

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemLookup")
													   .And.Contain("ResponseGroup", "ItemAttributes")
													   .And.Contain("ItemId", String.Join(",", ids));
			var clientMock = CreateClient(callback: callback);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.LookupAttributes(ids);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once());
		}

		[TestMethod]
		public async Task LookupAttributes_Should_Return_Elements_Returned_By_Client_GetItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var clientMock = CreateClient(expected);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			var actual = await provider.LookupAttributes(new[] { "id" });

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region LookupImages
		[TestMethod]
		public void LookupImages_Should_Throw_Exception_When_Ids_Is_Null()
		{
			// Arrange
			const string[] ids = null;

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.LookupImages(ids);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("ids");
		}

		[TestMethod]
		public async Task LookupImages_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			string[] ids = new[] { "id1", "id2" };

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemLookup")
													   .And.Contain("ResponseGroup", "Images")
													   .And.Contain("ItemId", String.Join(",", ids));
			var clientMock = CreateClient(callback: callback);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.LookupImages(ids);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once());
		}

		[TestMethod]
		public async Task LookupImages_Should_Return_Elements_Returned_By_Client_GetItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var clientMock = CreateClient(expected);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			var actual = await provider.LookupImages(new[] { "id" });

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static IAwsProvider CreateProvider(IAwsClient client = null)
		{
			return new AwsProvider(client ?? new Mock<IAwsClient>().Object);
		}

		private static Mock<IAwsClient> CreateClient(IEnumerable<XElement> items = null, Action<IStringDictionary> callback = null)
		{
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>()))
					  .ReturnsAsync(items ?? new[] { new XElement("Items") })
					  .Callback(callback ?? (d => { }));
			return clientMock;
		}
		#endregion
	}
}