using System;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlAwsProviderTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsClient_Is_Null()
		{
			// Arrange
			const IAwsClient provider = null;

			// Act
			Action action = () => new XmlAwsProvider(provider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("provider");
		}
		#endregion

		#region SearchItem
		[TestMethod]
		public void SearchItem_Should_Throw_Exception_When_Title_Is_Null()
		{
			// Arrange
			const string title = null;

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.SearchItem(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public void SearchItem_Should_Throw_Exception_When_Title_Is_Empty()
		{
			// Arrange
			const string title = "";

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.SearchItem(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public async Task SearchItem_Should_Pass_Arguments_To_AwsClient()
		{
			// Arrange
			const string title = "anyTitle";

			Action<IStringDictionary> callback = d => d.Should()
													  .Contain("Operation", "ItemSearch")
													  .And.Contain("SearchIndex", "Grocery")
													  .And.Contain("ResponseGroup", "Small")
													  .And.Contain("Title", title);
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>()))
					  .ReturnsAsync(new XElement("Items"))
					  .Callback(callback);

			IAwsProvider provider = CreateProvider();

			// Act
			await provider.SearchItem(title);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once());
		}
		#endregion

		#region LookupAttributes
		[TestMethod]
		public void LookupAttributes_Should_Throw_Exception_When_Id_Is_Null()
		{
			// Arrange
			const string id = null;

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.LookupAttributes(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}

		[TestMethod]
		public void LookupAttributes_Should_Throw_Exception_When_Id_Is_Empty()
		{
			// Arrange
			const string id = "";

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.LookupAttributes(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}
		#endregion

		#region LookupImages
		[TestMethod]
		public void LookupImages_Should_Throw_Exception_When_Id_Is_Null()
		{
			// Arrange
			const string id = null;

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.LookupImages(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}

		[TestMethod]
		public void LookupImages_Should_Throw_Exception_When_Id_Is_Empty()
		{
			// Arrange
			const string id = "";

			IAwsProvider provider = CreateProvider();

			// Act
			Func<Task> action = () => provider.LookupImages(id);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("id");
		}
		#endregion

		#region Helper methods
		private static IAwsProvider CreateProvider()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}