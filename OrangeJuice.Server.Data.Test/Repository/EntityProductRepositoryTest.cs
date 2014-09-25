using System.Linq;

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
		public void Search_Should_Return_Productss_Returned_By_ProductUnit_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var expected = new[] { new Product(), new Product() }.AsQueryable();

			var productUnitMock = new Mock<IProductUnit>();
			productUnitMock.Setup(u => u.Search(barcode, barcodeType)).Returns(expected);

			IProductRepository repository = new EntityProductRepository(productUnitMock.Object);

			// Act
			var actual = repository.Search(barcode, barcodeType);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[TestMethod]
		public void Search_Should_Call_ProductUnit_Search()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var productUnitMock = new Mock<IProductUnit>();

			IProductRepository repository = new EntityProductRepository(productUnitMock.Object);

			// Act
			repository.Search(barcode, barcodeType);

			// Assert
			productUnitMock.Verify(u => u.Search(barcode, barcodeType), Times.Once);
		}
		#endregion

		#region Dispose
		[TestMethod]
		public void Dispose_Should_Call_UserUnit_Dispose()
		{
			// Arrange
			var productUnitMock = new Mock<IProductUnit>();

			IProductRepository repository = new EntityProductRepository(productUnitMock.Object);

			// Act
			repository.Dispose();

			// Assert
			productUnitMock.Verify(u => u.Dispose(), Times.Once);
		}
		#endregion
	}
}