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
			const IAwsProviderFactory providerFactory = null;
			const IFoodDescriptionFactory foodDescriptionFactory = null;
			const IFilter<FoodDescription> foodDescriptionFilter = null;

			// Act
			Action action = () => new AwsFoodRepository(providerFactory, foodDescriptionFactory, foodDescriptionFilter);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("providerFactory");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_FoodDescriptionFactorys_Is_Null()
		{
			// Arrange
			IAwsProviderFactory providerFactory = new Mock<IAwsProviderFactory>().Object;
			const IFoodDescriptionFactory foodDescriptionFactory = null;
			const IFilter<FoodDescription> foodDescriptionFilter = null;

			// Act
			Action action = () => new AwsFoodRepository(providerFactory, foodDescriptionFactory, foodDescriptionFilter);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodDescriptionFactory");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_FoodDescriptionFilter_Is_Null()
		{
			// Arrange
			IAwsProviderFactory providerFactory = new Mock<IAwsProviderFactory>().Object;
			IFoodDescriptionFactory foodDescriptionFactory = new Mock<IFoodDescriptionFactory>().Object;
			const IFilter<FoodDescription> foodDescriptionFilter = null;

			// Act
			Action action = () => new AwsFoodRepository(providerFactory, foodDescriptionFactory, foodDescriptionFilter);

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
		public async Task SearchByTitle_Should_Pass_Title_To_AwsClient_SearhItems()
		{
			// Arrange
			const string title = "anyTitle";

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(title)).ReturnsAsync(Enumerable.Empty<XElement>());

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.SearchByTitle(title);

			// Assert
			providerMock.Verify(c => c.SearchItems(title), Times.Once());
		}

		// TODO: rewrite
		[TestMethod]
		public async Task SearchByTitle_Should_Pass_AttributesElement_ImagesElement_To_FoodDescriptionFactory_Returned_By_AwsClient()
		{
			// Arrange
			string[] ids = new[] { "id1", "id2" };

			XElement itemElement = new XElement("Item");
			var attributesTask = Task.FromResult(Enumerable.Repeat(new XElement("attributes"), 1));
			var imagesTask = Task.FromResult(Enumerable.Repeat(new XElement("images"), 1));

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems("anyTitle")).ReturnsAsync(new[] { itemElement });
			providerMock.Setup(c => c.LookupAttributes(ids)).Returns(attributesTask);
			providerMock.Setup(c => c.LookupImages(ids)).Returns(imagesTask);

			var factoryMock = new Mock<IFoodDescriptionFactory>();
			factoryMock.Setup(f => f.GetId(itemElement)).Returns("id1");
			factoryMock.Setup(f => f.Create(It.IsAny<XElement>(), It.IsAny<XElement>())).Returns(new FoodDescription());

			AwsFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			await repository.SearchByTitle("anyTitle");

			// Assert
			factoryMock.Verify(f => f.Create(attributesTask.Result.Single(), imagesTask.Result.Single()), Times.Once());
		}
		#endregion

		#region Helper methods
		private static AwsFoodRepository CreateRepository(
			IAwsProvider awsProvider = null,
			IFoodDescriptionFactory foodDescriptionFactory = null,
			IFilter<FoodDescription> foodDescriptionFilter = null)
		{
			var providerFactoryMock = new Mock<IAwsProviderFactory>();
			providerFactoryMock.Setup(c => c.Create()).Returns(awsProvider ?? new Mock<IAwsProvider>().Object);

			return new AwsFoodRepository(
				providerFactoryMock.Object,
				foodDescriptionFactory ?? new Mock<IFoodDescriptionFactory>(MockBehavior.Strict).Object,
				foodDescriptionFilter ?? new Mock<IFilter<FoodDescription>>().Object);
		}
		#endregion
	}
}