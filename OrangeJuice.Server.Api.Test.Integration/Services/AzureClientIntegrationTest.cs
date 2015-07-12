using System;
using System.Threading.Tasks;

using FluentAssertions;
using Newtonsoft.Json.Linq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using SimpleInjector;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	public class AzureClientIntegrationTest
	{
		[Theory]
		[InlineData(typeof(AzureClient))]
		[InlineData(typeof(FSharp.Services.AzureClient))]
		public async Task GetBlobFromContainer_Should_Return_Blob_When_Blob_Exists(Type type)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			IConfigurationProvider configurationProvider = container.GetInstance<IConfigurationProvider>();
			string containerName = configurationProvider.GetValue("blob:Products");

			IAzureClient client = (IAzureClient)container.GetInstance(type);

			// Act
			string content = await client.GetBlobFromContainer(containerName, "32456dc4-0473-4d5b-891f-b96efaf26ed2");

			// Assert
			content.Should().NotBeNullOrEmpty();
		}

		[Theory]
		[InlineData(typeof(AzureClient))]
		[InlineData(typeof(FSharp.Services.AzureClient))]
		public async Task GetBlobFromContainer_Should_Retur_Null_When_Blob_Does_Not_Exist(Type type)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			IConfigurationProvider configurationProvider = container.GetInstance<IConfigurationProvider>();
			string containerName = configurationProvider.GetValue("blob:Products");

			IAzureClient client = (IAzureClient)container.GetInstance(type);

			// Act
			string content = await client.GetBlobFromContainer(containerName, Guid.NewGuid().ToString());

			// Assert
			content.Should().BeNull();
		}

		[Theory]
		[InlineData(typeof(AzureClient))]
		[InlineData(typeof(FSharp.Services.AzureClient))]
		public void PutBlobToContainer_Should_Not_Throw_Exception_When_Saves_Content_To_Blob(Type type)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			IConfigurationProvider configurationProvider = container.GetInstance<IConfigurationProvider>();
			string containerName = configurationProvider.GetValue("blob:Products");

			IAzureClient client = (IAzureClient)container.GetInstance(type);

			string content = JObject.FromObject(new ProductDescriptor()).ToString();

			// Act
			Func<Task> func = () => client.PutBlobToContainer(containerName, Guid.NewGuid().ToString(), content);

			// Assert
			func.ShouldNotThrow();
		}

		[Theory]
		[InlineData(typeof(AzureClient))]
		[InlineData(typeof(FSharp.Services.AzureClient))]
		public void GetBlobUrl_Should_Return_BlobUrl_When_Blob_Exists(Type type)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			IConfigurationProvider configurationProvider = container.GetInstance<IConfigurationProvider>();
			string containerName = configurationProvider.GetValue("blob:Products");

			IAzureClient client = (IAzureClient)container.GetInstance(type);

			// Act
			Uri url = client.GetBlobUrl(containerName, "32456dc4-0473-4d5b-891f-b96efaf26ed2");

			// Assert
			url.Should().NotBeNull();
		}
	}
}