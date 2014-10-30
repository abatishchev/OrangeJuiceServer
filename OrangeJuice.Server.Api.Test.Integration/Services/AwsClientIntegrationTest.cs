using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.Practices.Unity;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	public class AwsClientIntegrationTest
	{
		[Fact]
		public async Task GetItems_Should_Return_Sequnce_Of_ProductDescriptor()
		{
			using (IUnityContainer container = ContainerConfig.CreateWebApiContainer())
			{
				// Arrange
				IAwsClient client = container.Resolve<IAwsClient>();
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
}