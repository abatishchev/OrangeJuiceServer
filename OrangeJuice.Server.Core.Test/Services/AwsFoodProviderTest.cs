using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task Search_Should_Pass_Items_To_FoodDescriptorFactory_Create()
		{
			// Arrange
			XElement[] elements = { new XElement("Item"), new XElement("Item") };

			var clientMock = CreateClient(elements);

			var factoryMock = new Mock<IFoodDescriptorFactory>();
			factoryMock.Setup(f => f.Create(It.IsIn(elements))).Returns(new FoodDescriptor());

			IFoodProvider provider = CreateProvider(clientMock.Object, factoryMock.Object);

			// Act
			(await provider.Search("titlle")).ToArray();

			// Assert
			factoryMock.Verify(f => f.Create(It.IsIn(elements)), Times.Exactly(elements.Length));
		}

		[TestMethod]
		public async Task Search_Should_Return_Descriptors_Returned_By_FoodDescriptorFactory_Create()
		{
			// Arrange
			XElement element1 = new XElement("Item1"), element2 = new XElement("Item2");
			FoodDescriptor descriptor1 = new FoodDescriptor(), descriptor2 = new FoodDescriptor();
			var expected = new[] { descriptor1, descriptor2 };

			var clientMock = CreateClient(new[] { element1, element2 });

			var factoryMock = new Mock<IFoodDescriptorFactory>();
			factoryMock.Setup(f => f.Create(element1)).Returns(descriptor1);
			factoryMock.Setup(f => f.Create(element2)).Returns(descriptor2);

			IFoodProvider provider = CreateProvider(clientMock.Object, factoryMock.Object);

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
		public async Task Lookup_Should_Pass_First_Item_To_FoodDescriptorFactory_Create()
		{
			// Arrange
			XElement[] elements = { new XElement("Item"), new XElement("Item") };

			var clientMock = CreateClient(elements);

			var factoryMock = new Mock<IFoodDescriptorFactory>();
			factoryMock.Setup(f => f.Create(It.IsIn(elements))).Returns(new FoodDescriptor());

			IFoodProvider provider = CreateProvider(clientMock.Object, factoryMock.Object);

			// Act
			await provider.Lookup("barcode", "barcodeType");

			// Assert
			factoryMock.Verify(f => f.Create(elements.First()), Times.Once);
		}

		[TestMethod]
		public async Task Lookup_Should_Return_Descriptor_Returned_By_FoodDescriptorFactory_Create()
		{
			// Arrange
			FoodDescriptor expected = new FoodDescriptor();

			var factoryMock = CreateFactory(expected);

			IFoodProvider provider = CreateProvider(factory: factoryMock.Object);

			// Act
			FoodDescriptor actual = await provider.Lookup("barcode", "barcodeType");

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IFoodProvider CreateProvider(IAwsClient client = null, IFoodDescriptorFactory factory = null)
		{
			return new AwsFoodProvider(
				client ?? CreateClient().Object,
				factory ?? CreateFactory().Object);
		}

		private static Mock<IAwsClient> CreateClient(IEnumerable<XElement> elements = null, Action<IStringDictionary> callback = null)
		{
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>()))
					  .ReturnsAsync(elements ?? new[] { new XElement("Items") })
					  .Callback(callback ?? (d => { }));
			return clientMock;
		}

		private static Mock<IFoodDescriptorFactory> CreateFactory(FoodDescriptor descriptor = null)
		{
			var factoryMock = new Mock<IFoodDescriptorFactory>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(descriptor ?? new FoodDescriptor());
			return factoryMock;
		}
		#endregion
	}
}