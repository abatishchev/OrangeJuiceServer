using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	[TestClass]
	public class AwsClientIntergrationTest
	{
		[TestMethod]
		public async Task GetItems_Should_Return_Sequnce_Of_ProductDescriptor()
		{
			using (IUnityContainer container = ContainerConfig.CreateContainer())
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