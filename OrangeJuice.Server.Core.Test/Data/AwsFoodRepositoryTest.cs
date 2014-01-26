using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class AwsFoodRepositoryTest
	{
		#region SearchTitle
		[TestMethod]
		public async Task SearchTitle_Should_Pass_Title_To_AwsProvider_SearhItems()
		{
			// Arrange
			const string title = "title";

			var providerMock = new Mock<IAwsProvider>();

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.SearchTitle(title);

			// Assert
			providerMock.Verify(c => c.SearchItems(title), Times.Once);
		}

		[TestMethod]
		public async Task SearchTitle_Should_Pass_Each_Item_Returned_By_AwsProvider_SearhItems_To_FoodDescriptionFactory_Create()
		{
			// Arrange
			const string title = "anyTitle";
			XElement element = new XElement("item");

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(title)).ReturnsAsync(new[] { element });
			var factoryMock = CreateFactory(element);

			IFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			(await repository.SearchTitle(title)).ToArray();

			// Assert
			factoryMock.Verify(f => f.Create(element), Times.Once);
		}
		#endregion

		#region SearchBarcode
		[TestMethod]
		public async Task SearchBarcode_Should_Pass_Barcode_And_BarcodeType_To_AwsProvider_ItemLookup()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var providerMock = new Mock<IAwsProvider>();

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.SearchBarcode(barcode, barcodeType);

			// Assert
			providerMock.Verify(c => c.ItemLookup(barcode, barcodeType.ToString()), Times.Once);
		}

		[TestMethod]
		public async Task SearchBarcode_Should_Pass_Each_Item_Returned_By_AwsProvider_SearhItems_To_FoodDescriptionFactory_Create()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			XElement element = new XElement("item");

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.ItemLookup(barcode, barcodeType.ToString())).ReturnsAsync(new[] { element });
			var factoryMock = CreateFactory(element);

			IFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			(await repository.SearchBarcode(barcode, barcodeType)).ToArray();

			// Assert
			factoryMock.Verify(f => f.Create(element), Times.Once);
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(IAwsProvider provider = null, IFoodDescriptionFactory factory = null)
		{
			return new AwsFoodRepository(
				provider ?? new Mock<IAwsProvider>().Object,
				factory ?? CreateFactory().Object);
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