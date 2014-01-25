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
			const string title = "anyTitle";

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(title)).ReturnsAsync(new XElement[0]);

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

			var factoryMock = new Mock<IFoodDescriptionFactory>();

			IFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			await repository.SearchTitle(title);

			// Assert
			factoryMock.Verify(f => f.Create(element), Times.Once);
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(IAwsProvider provider = null, IFoodDescriptionFactory factory = null)
		{
			return new AwsFoodRepository(
				provider ?? CreateProvider(),
				factory ?? new Mock<IFoodDescriptionFactory>().Object);
		}

		private static IAwsProvider CreateProvider()
		{
			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(It.IsAny<string>())).ReturnsAsync(new XElement[0]);
			return providerMock.Object;
		}
		#endregion
	}
}