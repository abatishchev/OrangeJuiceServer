using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Test.Repository
{
	[TestClass]
	public class EntityProductRepositoryTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Return_Rating_Returned_By_ProductUnit_Get()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			Product expected = new Product();

			var productUnitMock = new Mock<IProductUnit>();
			productUnitMock.Setup(u => u.Get(barcode, barcodeType)).ReturnsAsync(expected);

			IProductRepository repository = CreateRepository(productUnitMock.Object);

			// Act
			IProduct actual = await repository.Search(barcode, barcodeType);

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task Search_Should_Call_ProductUnit_Get()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var productUnitMock = new Mock<IProductUnit>();

			IProductRepository repository = CreateRepository(productUnitMock.Object);

			// Act
			await repository.Search(barcode, barcodeType);

			// Assert
			productUnitMock.Verify(u => u.Get(barcode, barcodeType), Times.Once);
		}
		#endregion

		#region Dispose
		[TestMethod]
		public void Dispose_Should_Call_UserUnit_Dispose()
		{
			// Arrange
			var productUnitMock = new Mock<IProductUnit>();

			IProductRepository repository = CreateRepository(productUnitMock.Object);

			// Act
			repository.Dispose();

			// Assert
			productUnitMock.Verify(u => u.Dispose(), Times.Once);
		}
		#endregion

		#region Helper methods
		private static IProductRepository CreateRepository(IProductUnit productUnit)
		{
			return new EntityProductRepository(
				productUnit ?? new Mock<IProductUnit>().Object);
		}
		#endregion
	}
}