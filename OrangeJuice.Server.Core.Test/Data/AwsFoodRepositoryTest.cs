using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

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
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsClientFactory_Is_Null()
		{
			// Arrange
			const IAwsProvider provider = null;
			const IFoodDescriptionFactory foodDescriptionFactory = null;
			const IFilter<FoodDescription> foodDescriptionFilter = null;

			// Act
			Action action = () => new AwsFoodRepository(provider, foodDescriptionFactory, foodDescriptionFilter);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("provider");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_FoodDescriptionFactorys_Is_Null()
		{
			// Arrange
			IAwsProvider provider = new Mock<IAwsProvider>().Object;
			const IFoodDescriptionFactory foodDescriptionFactory = null;
			const IFilter<FoodDescription> foodDescriptionFilter = null;

			// Act
			Action action = () => new AwsFoodRepository(provider, foodDescriptionFactory, foodDescriptionFilter);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodDescriptionFactory");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_FoodDescriptionFilter_Is_Null()
		{
			// Arrange
			IAwsProvider provider = new Mock<IAwsProvider>().Object;
			IFoodDescriptionFactory foodDescriptionFactory = new Mock<IFoodDescriptionFactory>().Object;
			const IFilter<FoodDescription> foodDescriptionFilter = null;

			// Act
			Action action = () => new AwsFoodRepository(provider, foodDescriptionFactory, foodDescriptionFilter);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodDescriptionFilter");
		}
		#endregion

		#region SearchByTitle
		[TestMethod]
		public void SearchByTitle_Should_Throw_Exception_When_Title_Is_Null()
		{
			// Arrange
			const string title = null;
			var repository = CreateRepository();

			// Act
			Func<Task> action = () => repository.SearchByTitle(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public void SearchByTitle_Should_Throw_Exception_When_Title_Is_Empty()
		{
			// Arrange
			const string title = "";
			var repository = CreateRepository();

			// Act
			Func<Task> action = () => repository.SearchByTitle(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public async Task SearchByTitle_Should_Pass_Title_To_AwsClient_SearhItem()
		{
			// Arrange
			const string title = "anyTitle";

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItem(title)).ReturnsAsync(Enumerable.Empty<XElement>());

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.SearchByTitle(title);

			// Assert
			providerMock.Verify(c => c.SearchItem(title), Times.Once());
		}

		// ReSharper disable ImplicitlyCapturedClosure
		[TestMethod]
		public async Task SearchByTitle_Should_Pass_Id_AttributesTask_ImagesTask_To_FoodDescriptionFactory_Returned_By_AwsClient()
		{
			// Arrange
			const string id = "id";
			Task<XElement> attributesTask = Task.FromResult(new XElement("attributes"));
			Task<XElement> imagesTask = Task.FromResult(new XElement("images"));

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItem(id)).ReturnsAsync(new[] { new XElement("Item", new XElement("ASIN", id)) });
			providerMock.Setup(c => c.LookupAttributes(id)).Returns(attributesTask);
			providerMock.Setup(c => c.LookupImages(id)).Returns(imagesTask);

			var factoryMock = new Mock<IFoodDescriptionFactory>();
			factoryMock.Setup(f => f.Create(id, attributesTask, imagesTask)).ReturnsAsync(new FoodDescription());

			AwsFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			await repository.SearchByTitle(id);

			// Assert
			factoryMock.Verify(f => f.Create(id, attributesTask, imagesTask), Times.Once());
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(
			IAwsProvider provider = null,
			IFoodDescriptionFactory foodDescriptionFactory = null,
			IFilter<FoodDescription> foodDescriptionFilter = null)
		{
			return new AwsFoodRepository(
				provider ?? new Mock<IAwsProvider>(MockBehavior.Strict).Object,
				foodDescriptionFactory ?? new Mock<IFoodDescriptionFactory>(MockBehavior.Strict).Object,
				foodDescriptionFilter ?? new Mock<IFilter<FoodDescription>>().Object);
		}
		#endregion
	}
}