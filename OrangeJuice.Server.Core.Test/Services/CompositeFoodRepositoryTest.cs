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
	public class CompositeFoodRepositoryTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Title_To_FoodProvider_Search()
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

		[TestMethod]
		public async Task Search_Should_Return_FoodDescriptors_From_First_FoodProvider_Search_Which_Not_Returned_Null()
		{
			// Arrange
			const string title = "title";
			FoodDescriptor[] expected = { new FoodDescriptor(), new FoodDescriptor() };

			var providerMock1 = new Mock<IFoodProvider>();
			providerMock1.Setup(p => p.Search(title)).ReturnsAsync(null);

			var providerMock2 = new Mock<IFoodProvider>();
			providerMock2.Setup(p => p.Search(title)).ReturnsAsync(expected);

			var providerMock3 = new Mock<IFoodProvider>();
			providerMock3.Setup(p => p.Search(It.IsAny<string>())).Throws<InvalidOperationException>();

			IFoodRepository repository = CreateRepository(providerMock1.Object, providerMock2.Object);

			// Act
			var actual = await repository.Search(title);

			// Assert
			actual.ShouldBeEquivalentTo(expected);

			providerMock1.Verify(p => p.Search(title), Times.Once);
			providerMock2.Verify(p => p.Search(title), Times.Once);
			providerMock3.Verify(p => p.Search(It.IsAny<string>()), Times.Never);
		}
		#endregion

		#region Lookup
		[TestMethod]
		public async Task Lookup_Should_Pass_Barcode_And_BarcodeType_To_FoodProvider_Lookup()
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

		[TestMethod]
		public async Task Lookup_Should_Return_FoodDescriptors_From_First_FoodProvider_Lookup_Which_Not_Returned_Null()
		{
			// Arrange
			const string barcode = "barcode";
			const BarcodeType barcodeType = BarcodeType.EAN;
			FoodDescriptor expected = new FoodDescriptor();

			var providerMock1 = new Mock<IFoodProvider>();
			providerMock1.Setup(p => p.Lookup(barcode, barcodeType.ToString())).ReturnsAsync(null);

			var providerMock2 = new Mock<IFoodProvider>();
			providerMock2.Setup(p => p.Lookup(barcode, barcodeType.ToString())).ReturnsAsync(expected);

			var providerMock3 = new Mock<IFoodProvider>();
			providerMock3.Setup(p => p.Lookup(It.IsAny<string>(), It.IsAny<string>())).Throws<InvalidOperationException>();

			IFoodRepository repository = CreateRepository(providerMock1.Object, providerMock2.Object, providerMock3.Object);

			// Act
			FoodDescriptor actual = await repository.Lookup(barcode, barcodeType);

			// Assert
			actual.ShouldBeEquivalentTo(expected);

			providerMock1.Verify(p => p.Lookup(barcode, barcodeType.ToString()), Times.Once);
			providerMock2.Verify(p => p.Lookup(barcode, barcodeType.ToString()), Times.Once);
			providerMock3.Verify(p => p.Lookup(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}
		#endregion

		#region Helper methods
		private static IFoodRepository CreateRepository(params IFoodProvider[] providers)
		{
			return new CompositeFoodRepository(providers, CreteValidator());
		}

		private static IValidator<FoodDescriptor> CreteValidator()
		{
			var validatorMock = new Mock<IValidator<FoodDescriptor>>();
			validatorMock.Setup(v => v.IsValid(It.IsAny<FoodDescriptor>())).Returns<FoodDescriptor>(d => d != null);
			return validatorMock.Object;
		}
		#endregion
	}
}