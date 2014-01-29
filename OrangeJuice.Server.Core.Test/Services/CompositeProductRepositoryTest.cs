using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class CompositeProductRepositoryTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Title_To_ProductProvider_SearchTitle()
		{
			// Arrange
			const string title = "title";

			var providerMock = new Mock<IProductProvider>();
			IProductRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.Search(title);

			// Assert
			providerMock.Verify(c => c.SearchTitle(title), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_First_Not_Null_ProductDescriptors_Returned_By_ProductProvider_SearchTitle()
		{
			// Arrange
			const string title = "title";
			ProductDescriptor[] expected = { new ProductDescriptor(), new ProductDescriptor() };

			var providerMock1 = new Mock<IProductProvider>();
			providerMock1.Setup(p => p.SearchTitle(title)).ReturnsAsync(null);

			var providerMock2 = new Mock<IProductProvider>();
			providerMock2.Setup(p => p.SearchTitle(title)).ReturnsAsync(expected);

			var providerMock3 = new Mock<IProductProvider>();
			providerMock3.Setup(p => p.SearchTitle(It.IsAny<string>())).Throws<InvalidOperationException>();

			IProductRepository repository = CreateRepository(providerMock1.Object, providerMock2.Object);

			// Act
			var actual = await repository.Search(title);

			// Assert
			actual.ShouldBeEquivalentTo(expected);

			providerMock1.Verify(p => p.SearchTitle(title), Times.Once);
			providerMock2.Verify(p => p.SearchTitle(title), Times.Once);
			providerMock3.Verify(p => p.SearchTitle(It.IsAny<string>()), Times.Never);
		}
		#endregion

		#region Lookup
		[TestMethod]
		public async Task Lookup_Should_Pass_Barcode_And_BarcodeType_To_ProductProvider_SearchBarcode()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;

			var providerMock = new Mock<IProductProvider>();
			IProductRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.Lookup(barcode, barcodeType);

			// Assert
			providerMock.Verify(c => c.SearchBarcode(barcode, barcodeType.ToString()), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Return_First_Not_Null_ProductDescriptors_Returned_By_ProductProvider_SearchBarcode()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			ProductDescriptor expected = new ProductDescriptor();

			var providerMock1 = new Mock<IProductProvider>();
			providerMock1.Setup(p => p.SearchBarcode(barcode, barcodeType.ToString())).ReturnsAsync(null);

			var providerMock2 = new Mock<IProductProvider>();
			providerMock2.Setup(p => p.SearchBarcode(barcode, barcodeType.ToString())).ReturnsAsync(expected);

			var providerMock3 = new Mock<IProductProvider>();
			providerMock3.Setup(p => p.SearchBarcode(It.IsAny<string>(), It.IsAny<string>())).Throws<InvalidOperationException>();

			IProductRepository repository = CreateRepository(providerMock1.Object, providerMock2.Object, providerMock3.Object);

			// Act
			ProductDescriptor actual = await repository.Lookup(barcode, barcodeType);

			// Assert
			actual.ShouldBeEquivalentTo(expected);

			providerMock1.Verify(p => p.SearchBarcode(barcode, barcodeType.ToString()), Times.Once);
			providerMock2.Verify(p => p.SearchBarcode(barcode, barcodeType.ToString()), Times.Once);
			providerMock3.Verify(p => p.SearchBarcode(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}
		#endregion

		#region Helper methods
		private static IProductRepository CreateRepository(params IProductProvider[] providers)
		{
			return new CompositeProductRepository(providers, CreteValidator());
		}

		private static IValidator<ProductDescriptor> CreteValidator()
		{
			var validatorMock = new Mock<IValidator<ProductDescriptor>>();
			validatorMock.Setup(v => v.IsValid(It.IsAny<ProductDescriptor>())).Returns<ProductDescriptor>(d => d != null);
			return validatorMock.Object;
		}
		#endregion
	}
}