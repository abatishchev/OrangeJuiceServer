﻿using System;
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
	public class AwsProductProviderTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemLookup")
													   .And.Contain("SearchIndex", "Grocery")
													   .And.Contain("ResponseGroup", "Images,ItemAttributes")
													   .And.Contain("IdType", barcodeType.ToString())
													   .And.Contain("ItemId", barcode);
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>())).ReturnsAsync(new[] { new XElement("Items") }).Callback(callback);

			IAwsProductProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.Search(barcode, barcodeType);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Pass_First_Item_To_ProductDescriptorFactory_Create()
		{
			// Arrange
			XElement[] elements = { new XElement("Item"), new XElement("Item") };

			IAwsClient client = CreateClient(elements);

			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsIn(elements))).Returns(new ProductDescriptor());

			IAwsProductProvider provider = CreateProvider(client, factoryMock.Object);

			// Act
			await provider.Search("barcode", BarcodeType.EAN);

			// Assert
			factoryMock.Verify(f => f.Create(elements.First()), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_Descriptor_Returned_By_ProductDescriptorFactory_Create()
		{
			// Arrange
			ProductDescriptor expected = new ProductDescriptor();

			IAwsClient client = CreateClient();

			var factoryMock = new Mock<IFactory<XElement, ProductDescriptor>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(expected);

			IAwsProductProvider provider = CreateProvider(client, factoryMock.Object);

			// Act
			ProductDescriptor actual = await provider.Search("barcode", BarcodeType.EAN);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IAwsProductProvider CreateProvider(IAwsClient client = null, IFactory<XElement, ProductDescriptor> factory = null)
		{
			return new AwsProductProvider(
				client ?? Mock.Of<IAwsClient>(),
				factory ?? Mock.Of<IFactory<XElement, ProductDescriptor>>());
		}

		private static IAwsClient CreateClient(IEnumerable<XElement> elements = null)
		{
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>())).ReturnsAsync(elements ?? new[] { new XElement("Items") });
			return clientMock.Object;
		}
		#endregion
	}
}