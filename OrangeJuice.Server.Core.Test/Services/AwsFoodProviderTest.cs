using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsFoodProviderTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			const string title = "titlle";

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemSearch")
													   .And.Contain("SearchIndex", "Grocery")
													   .And.Contain("ResponseGroup", "Images,ItemAttributes")
													   .And.Contain("Keywords", title);
			var clientMock = CreateClient(callback: callback);

			IFoodProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.Search(title);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_Elements_Returned_By_Client_GetItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var clientMock = CreateClient(expected);

			IFoodProvider provider = CreateProvider(clientMock.Object);

			// Act
			var actual = await provider.Search("titlle");

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Lookup
		[TestMethod]
		public async Task Lookup_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			const string barcode = "barcode";
			const string barcodeType = "barcodeType";

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemLookup")
													   .And.Contain("SearchIndex", "Grocery")
													   .And.Contain("ResponseGroup", "Images,ItemAttributes")
													   .And.Contain("IdType", barcodeType)
													   .And.Contain("ItemId", barcode);
			var clientMock = CreateClient(callback: callback);

			IFoodProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.Lookup(barcode, barcodeType);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once);
		}

		[TestMethod]
		public async Task Lookup_Should_Return_Elements_Returned_By_Client_GetItems()
		{
			// Arrange
			var expected = new FoodDescription();

			var factoryMock = CreateFactory(description: expected);

			IFoodProvider provider = CreateProvider(factory: factoryMock.Object);

			// Act
			FoodDescription actual = await provider.Lookup("barcode", "barcodeType");

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IFoodProvider CreateProvider(IAwsClient client = null, IFoodDescriptionFactory factory = null)
		{
			return new AwsFoodProvider(
				client ?? CreateClient().Object,
				factory ?? CreateFactory().Object);
		}

		private static Mock<IAwsClient> CreateClient(ICollection<XElement> items = null, Action<IStringDictionary> callback = null)
		{
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>()))
					  .ReturnsAsync(items ?? new[] { new XElement("Items") })
					  .Callback(callback ?? (d => { }));
			return clientMock;
		}

		private static Mock<IFoodDescriptionFactory> CreateFactory(XElement element = null, FoodDescription description = null)
		{
			var factoryMock = new Mock<IFoodDescriptionFactory>();
			factoryMock.Setup(f => f.Create(element ?? It.IsAny<XElement>())).Returns(description ?? new FoodDescription());
			return factoryMock;
		}
		#endregion
	}
}