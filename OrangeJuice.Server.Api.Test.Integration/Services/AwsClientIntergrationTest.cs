using System.Collections.Generic;
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
		public async Task GetItems_Should_Return_Sequnce_Of_XElement()
		{
			using (IUnityContainer container = ContainerConfig.CreateContainer())
			{
				// Arrange
				IAwsClient client = container.Resolve<IAwsClient>();
				var searchCriteria = new ProductDescriptorSearchCriteria
				{
					Operation = "ItemSearch",
					SearchIndex = "Grocery",
					ResponseGroup = new[] { "Small" },
					Title = "Coca-Cola"
				};

				// Act
				var items = await client.GetItems(args);

				// Assert
				items.Should().NotBeEmpty();
			}
		}
	}
}