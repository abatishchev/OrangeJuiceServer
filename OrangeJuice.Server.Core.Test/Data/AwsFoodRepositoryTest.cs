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
		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_Title_To_AwsProvider_SearhItems()
		{
			// Arrange
			const string title = "anyTitle";

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(title)).ReturnsAsync(new XElement[0]);
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new XElement[0]);
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new XElement[0]);

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.Search(title);

			// Assert
			providerMock.Verify(c => c.SearchItems(title), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Pass_ItemElement_To_IdSelector_GetId_For_Each_ItemElement_Returned_By_AwsProvider_SearchItems()
		{
			// Arrange
			XElement itemElement = new XElement("Item");

			var provider = CreateProvider(itemElement);

			var idSelectorMock = new Mock<IIdSelector>();

			AwsFoodRepository repository = CreateRepository(provider, idSelector: idSelectorMock.Object);

			// Act
			await repository.Search("anyTitle");

			// Assert
			idSelectorMock.Verify(f => f.GetId(itemElement), Times.Once);
		}

		[TestMethod]
		public async Task Search_Should_Pass_AttributesElement_ImagesElement_To_FoodDescriptionFactory_Returned_By_AwsProvider_SearchItems()
		{
			// Arrange
			XElement itemElement = new XElement("Item");
			XElement attributesElement = new XElement("attributes");
			XElement imagesElement = new XElement("images");

			var provider = CreateProvider(itemElement, attributesElement, imagesElement);

			var factoryMock = new Mock<IFoodDescriptionFactory>();
			factoryMock.Setup(f => f.Create(It.IsAny<string>(), attributesElement, imagesElement));

			AwsFoodRepository repository = CreateRepository(provider, factoryMock.Object);

			// Act
			await repository.Search("anyTitle");

			// Assert
			factoryMock.Verify(f => f.Create(It.IsAny<string>(), attributesElement, imagesElement), Times.Once);
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(
			IAwsProvider provider = null,
			IFoodDescriptionFactory foodDescriptionFactory = null,
			IFilter<FoodDescription> foodDescriptionFilter = null,
			IIdSelector idSelector = null)
		{
			return new AwsFoodRepository(
				provider ?? CreateProvider(),
				foodDescriptionFactory ?? new Mock<IFoodDescriptionFactory>().Object,
				foodDescriptionFilter ?? new Mock<IFilter<FoodDescription>>().Object,
				idSelector ?? CreateSelector());
		}

		private static IAwsProvider CreateProvider(XElement itemElement = null, XElement attributesElement = null, XElement imagesElement = null)
		{
			var providerMock = new Mock<IAwsProvider>();

			providerMock.Setup(c => c.SearchItems(It.IsAny<string>())).ReturnsAsync(itemElement != null ? new[] { itemElement } : new XElement[0]);
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(attributesElement != null ? new[] { attributesElement } : new XElement[0]);
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(imagesElement != null ? new[] { imagesElement } : new XElement[0]);

			return providerMock.Object;
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