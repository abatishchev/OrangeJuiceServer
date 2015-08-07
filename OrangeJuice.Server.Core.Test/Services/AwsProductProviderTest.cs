using System;
using System.Linq;
using System.Xml.Linq;
using Factory;
using Moq;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Test.Services
{
	public class AwsProductProviderTest
	{
		#region Search
		[Fact]
		public void Search_Should_Pass_SearchCriteria_To_Client()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			Func<AwsProductSearchCriteria, bool> verify = c =>
				c.Operation == "ItemLookup" &&
				c.SearchIndex == "Grocery" &&
				c.ResponseGroups.SequenceEqual(new[] { "Images", "ItemAttributes" }) &&
				c.IdType == barcodeType.ToString() &&
				c.ItemId == barcode;
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.Is<AwsProductSearchCriteria>(p => verify(p)))).ReturnsAsync(new[] { new XElement("Item") });

			IAwsProductProvider provider = CreateProvider(clientMock.Object);

			// Act
			provider.Search(barcode, barcodeType);

			// Assert
			clientMock.VerifyAll();
		}
		#endregion

		#region Helper methods
		private static IAwsProductProvider CreateProvider(IAwsClient client = null, IFactory<ProductDescriptor, XElement> factory = null)
		{
			return new AwsProductProvider(
				client ?? Mock.Of<IAwsClient>(),
				factory ?? CreateFactory());
		}

		private static IFactory<ProductDescriptor, XElement> CreateFactory(ProductDescriptor descriptor = null)
		{
			var factoryMock = new Mock<IFactory<ProductDescriptor, XElement>>();
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>())).Returns(descriptor ?? new ProductDescriptor());
			return factoryMock.Object;
		}
		#endregion
	}
}