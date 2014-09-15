using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsProductProviderTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_SearchCriteria_To_Client()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			Func<ProductDescriptorSearchCriteria, bool> verify = c =>
			    c.Operation == "ItemLookup" &&
			    c.SearchIndex == "Grocery" &&
			    c.ResponseGroups.SequenceEqual(new[] { "Images", "ItemAttributes" }) &&
			    c.IdType == barcodeType.ToString() &&
			    c.ItemId == barcode;
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.Is<ProductDescriptorSearchCriteria>(p => verify(p)))).ReturnsAsync(new[] { new ProductDescriptor() });

			IAwsProductProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.Search(barcode, barcodeType);

			// Assert
			clientMock.VerifyAll();
		}
		#endregion

		#region Helper methods
		private static IAwsProductProvider CreateProvider(IAwsClient client = null)
		{
			return new AwsProductProvider(client ?? Mock.Of<IAwsClient>());
		}
		#endregion
	}
}