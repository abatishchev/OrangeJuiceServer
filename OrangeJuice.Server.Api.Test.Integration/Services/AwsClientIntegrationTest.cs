using System.Threading.Tasks;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using SimpleInjector;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	public class AwsClientIntegrationTest
	{
		[Fact]
		public async Task GetItems_Should_Return_Sequnce_Of_ProductDescriptor()
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();
			IAwsClient client = container.GetInstance<IAwsClient>();
			var searchCriteria = new ProductDescriptorSearchCriteria
			{
				Operation = "ItemLookup",
				ResponseGroups = new[] { "Small" },
				SearchIndex = "Grocery",
				ItemId = "0747599330971",
				IdType = "EAN"
			};

			// Act
			var items = await client.GetItems(searchCriteria);

			// Assert
			items.Should().NotBeEmpty();
		}
	}
}