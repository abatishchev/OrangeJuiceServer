using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class AwsFoodRepositoryTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Title_To_AwsProvider_Searh()
		{
			// Arrange
			const string title = "title";

			var providerMock = new Mock<IFoodProvider>();

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.Search(title);

			// Assert
			providerMock.Verify(c => c.Search(title), Times.Once);
		}
		#endregion

		#region Lookup
		[TestMethod]
		public async Task Search_Should_Pass_Barcode_And_BarcodeType_To_AwsProvider_Lookup()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var providerMock = new Mock<IFoodProvider>();

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.Lookup(barcode, barcodeType);

			// Assert
			providerMock.Verify(c => c.Lookup(barcode, barcodeType.ToString()), Times.Once);
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(IFoodProvider provider = null)
		{
			return new AwsFoodRepository(
				provider != null ? new[] { new Mock<IFoodProvider>().Object } : new IFoodProvider[0]);
		}
		#endregion
	}
}