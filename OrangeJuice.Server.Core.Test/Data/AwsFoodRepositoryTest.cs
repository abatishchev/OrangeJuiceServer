﻿using System;
using System.Collections.Generic;
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
			providerMock.Setup(c => c.SearchItems(title)).ReturnsAsync(new XElement[0]);
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new XElement[0]);
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new XElement[0]);

			IFoodRepository repository = CreateRepository(providerMock.Object);

			// Act
			await repository.SearchByTitle(title);

			// Assert
			providerMock.Verify(c => c.SearchItems(title), Times.Once());
		}

		[TestMethod]
		public async Task SearchByTitle_Should_Pass_ItemElement_To_FoodDescriptionFactory_GetId_For_Each_ItemElement_Returned_By_AwsProvider_SearchItems()
		{
			// Arrange
			XElement itemElement = new XElement("Item");
			XElement attributesElement = new XElement("attributes");
			XElement imagesElement = new XElement("images");

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(It.IsAny<string>())).ReturnsAsync(new[] { itemElement });
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new[] { attributesElement });
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new[] { imagesElement });

			var factoryMock = new Mock<IFoodDescriptionFactory>();

			AwsFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			await repository.SearchByTitle("anyTitle");

			// Assert
			factoryMock.Verify(f => f.GetId(itemElement), Times.Once());
		}

		[TestMethod]
		public async Task SearchByTitle_Should_Pass_AttributesElement_ImagesElement_To_FoodDescriptionFactory_Returned_By_AwsProvider_SearchItems()
		{
			// Arrange
			XElement itemElement = new XElement("Item");
			XElement attributesElement = new XElement("attributes");
			XElement imagesElement = new XElement("images");

			var providerMock = new Mock<IAwsProvider>();
			providerMock.Setup(c => c.SearchItems(It.IsAny<string>())).ReturnsAsync(new[] { itemElement });
			providerMock.Setup(c => c.LookupAttributes(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new[] { attributesElement });
			providerMock.Setup(c => c.LookupImages(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new[] { imagesElement });

			var factoryMock = new Mock<IFoodDescriptionFactory>();

			AwsFoodRepository repository = CreateRepository(providerMock.Object, factoryMock.Object);

			// Act
			await repository.SearchByTitle("anyTitle");

			// Assert
			factoryMock.Verify(f => f.Create(attributesElement, imagesElement), Times.Once());
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
				foodDescriptionFactory ?? new Mock<IFoodDescriptionFactory>().Object,
				foodDescriptionFilter ?? new Mock<IFilter<FoodDescription>>().Object);
		}
		#endregion
	}
}