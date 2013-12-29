using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class AwsFoodRepositoryTest
	{
		#region SearchByTitle
		[TestMethod]
		public async Task SearchByTitle_Should_Call_ProviderFactory_Create()
		{
			// Arrange
			const string title = "anyTitle";

			var providerMock = CreateProvider();

			var providerFactoryMock = new Mock<IFactory<IAwsProvider>>();
			providerFactoryMock.Setup(f => f.Create()).Returns(providerMock.Object);

			IFoodRepository repository = CreateRepository(providerFactoryMock.Object);

			// Act
			await repository.SearchByTitle(title);

			// Assert
			providerFactoryMock.Verify(f => f.Create(), Times.Once);
		}

		[TestMethod]
		public async Task SearchByTitle_Should_Pass_Title_To_AwsProvider_SearhItems()
		{
			// Arrange
			const string title = "anyTitle";

			var providerMock = CreateProvider();
			providerMock.Setup(c => c.SearchItems(title)).ReturnsAsync(new XElement[0]);
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new XElement[0]);
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new XElement[0]);

			var providerFactoryMock = new Mock<IFactory<IAwsProvider>>();
			providerFactoryMock.Setup(f => f.Create()).Returns(providerMock.Object);

			IFoodRepository repository = CreateRepository(providerFactoryMock.Object);

			// Act
			await repository.SearchByTitle(title);

			// Assert
			providerMock.Verify(c => c.SearchItems(title), Times.Once);
		}

		[TestMethod]
		public async Task SearchByTitle_Should_Pass_ItemElement_To_IdSelector_GetId_For_Each_ItemElement_Returned_By_AwsProvider_SearchItems()
		{
			// Arrange
			XElement itemElement = new XElement("Item");

			var provider = CreateProvider(itemElement);
			var providerFactory = CreateProviderFactory(provider.Object);

			var idSelectorMock = new Mock<IIdSelector>();

			AwsFoodRepository repository = CreateRepository(providerFactory, idSelector: idSelectorMock.Object);

			// Act
			await repository.SearchByTitle("anyTitle");

			// Assert
			idSelectorMock.Verify(f => f.GetId(itemElement), Times.Once);
		}

		[TestMethod]
		public async Task SearchByTitle_Should_Pass_AttributesElement_ImagesElement_To_FoodDescriptionFactory_Returned_By_AwsProvider_SearchItems()
		{
			// Arrange
			XElement itemElement = new XElement("Item");
			XElement attributesElement = new XElement("attributes");
			XElement imagesElement = new XElement("images");

			var provider = CreateProvider(itemElement, attributesElement, imagesElement);
			var providerFactory = CreateProviderFactory(provider.Object);

			var factoryMock = new Mock<IFoodDescriptionFactory>();
			factoryMock.Setup(f => f.Create(It.IsAny<string>(), attributesElement, imagesElement));

			AwsFoodRepository repository = CreateRepository(providerFactory, factoryMock.Object);

			// Act
			await repository.SearchByTitle("anyTitle");

			// Assert
			factoryMock.Verify(f => f.Create(It.IsAny<string>(), attributesElement, imagesElement), Times.Once);
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(
			IFactory<IAwsProvider> providerFactory = null,
			IFoodDescriptionFactory foodDescriptionFactory = null,
			IFilter<FoodDescription> foodDescriptionFilter = null,
			IIdSelector idSelector = null)
		{
			return new AwsFoodRepository(
				providerFactory ?? CreateProviderFactory(),
				foodDescriptionFactory ?? new Mock<IFoodDescriptionFactory>().Object,
				foodDescriptionFilter ?? new Mock<IFilter<FoodDescription>>().Object,
				idSelector ?? CreateSelector());
		}

		private static IFactory<IAwsProvider> CreateProviderFactory(IAwsProvider provider = null)
		{
			var factoryMock = new Mock<IFactory<IAwsProvider>>();
			factoryMock.Setup(f => f.Create()).Returns(provider ?? CreateProvider().Object);
			return factoryMock.Object;
		}

		private static Mock<IAwsProvider> CreateProvider(XElement itemElement = null, XElement attributesElement = null, XElement imagesElement = null)
		{
			var providerMock = new Mock<IAwsProvider>();

			providerMock.Setup(c => c.SearchItems(It.IsAny<string>())).ReturnsAsync(itemElement != null ? new[] { itemElement } : new XElement[0]);
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(attributesElement != null ? new[] { attributesElement } : new XElement[0]);
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(imagesElement != null ? new[] { imagesElement } : new XElement[0]);

			return providerMock;
		}

		private static IIdSelector CreateSelector(string id = null, XElement element = null)
		{
			var idSelectorMock = new Mock<IIdSelector>();
			idSelectorMock.Setup(s => s.GetId(element ?? It.IsAny<XElement>())).Returns(id ?? "anyId");
			return idSelectorMock.Object;
		}

		#endregion
	}
}