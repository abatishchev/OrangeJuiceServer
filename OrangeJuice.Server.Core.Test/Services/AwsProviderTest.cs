using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsProviderTest
	{
		#region SearchItems
		[TestMethod]
		public async Task SearchItems_Should_Pass_Arguments_To_Client()
		{
			// Arrange
			const string title = "anyTitle";

			Action<IStringDictionary> callback = d => d.Should()
													   .Contain("Operation", "ItemSearch")
													   .And.Contain("SearchIndex", "Grocery")
													   .And.Contain("ResponseGroup", "Images,ItemAttributes")
													   .And.Contain("Keywords", title);
			var clientMock = CreateClient(callback: callback);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			await provider.SearchItems(title);

			// Assert
			clientMock.Verify(b => b.GetItems(It.IsAny<IStringDictionary>()), Times.Once());
		}

		[TestMethod]
		public async Task SearchItems_Should_Return_Elements_Returned_By_Client_GetItems()
		{
			// Arrange
			var expected = new[] { new XElement("Items") };
			var clientMock = CreateClient(expected);

			IAwsProvider provider = CreateProvider(clientMock.Object);

			// Act
			var actual = await provider.SearchItems("anyTitle");

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static IAwsProvider CreateProvider(IAwsClient client = null)
		{
			return new AwsProvider(
				client ?? new Mock<IAwsClient>().Object);
		}

		private static Mock<IAwsClient> CreateClient(ICollection<XElement> items = null, Action<IStringDictionary> callback = null)
		{
			var clientMock = new Mock<IAwsClient>();
			clientMock.Setup(b => b.GetItems(It.IsAny<IStringDictionary>()))
					  .ReturnsAsync(items ?? new[] { new XElement("Items") })
					  .Callback(callback ?? (d => { }));
			return clientMock;
		}
		#endregion
	}
}