using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	[TestClass]
	public class AwsClientIntergrationTest
	{
		[TestMethod]
		public async Task GetItems_Should_Return_Items()
		{
			using (IUnityContainer container = UnityConfig.InitializeContainer())
			{
				// Arrange
				IAwsClient client = container.Resolve<IAwsClient>();
				var args = new Dictionary<string, string>
				{
					{ "Operation", "ItemSearch" },
					{ "SearchIndex", "Grocery" },
					{ "ResponseGroup", "Small" },
					{ "Title", "Coca-Cola" }
				};

				// Act
				var items = await client.GetItems(args);

				// Assert
				items.Should().NotBeEmpty();
			}
		}
	}
}