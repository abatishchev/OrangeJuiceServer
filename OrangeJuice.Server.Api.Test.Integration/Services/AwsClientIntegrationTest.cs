using System;
using System.Threading.Tasks;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using SimpleInjector;

using Xunit.Extensions;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	public class AwsClientIntegrationTest
	{
		[Theory]
		[InlineData(typeof(XmlAwsClient))]
		[InlineData(typeof(FSharp.Services.XmlAwsClient))]
		public async Task GetItems_Should_Return_Sequnce_Of_ProductDescriptor(Type type)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();
			IAwsClient client = (IAwsClient)container.GetInstance(type);
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