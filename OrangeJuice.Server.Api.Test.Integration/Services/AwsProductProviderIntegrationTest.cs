using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using SimpleInjector;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	public class AwsProductProviderIntegrationTest
	{
		[Fact]
		public async Task Search_Should_Return_ProductDescriptor()
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();
			IAwsProductProvider provider = container.GetInstance<IAwsProductProvider>();

			// Act
			ProductDescriptor descriptor = (await provider.Search("0747599330971", BarcodeType.EAN)).FirstOrDefault(p => p.SourceProductId == "B00HSQEETM");

			// Assert
			descriptor.Should().NotBeNull();

			descriptor.SourceProductId.Should().Be("B00HSQEETM");
			descriptor.Title.Should().Be("Ghirardelli Valentine's Chocolate Squares Premium Chocolate Assortment");
			descriptor.Brand.Should().Be("Ghirardelli");

			descriptor.SmallImageUrl.Should().NotBeNullOrEmpty();
			descriptor.MediumImageUrl.Should().NotBeNullOrEmpty();
			descriptor.LargeImageUrl.Should().NotBeNullOrEmpty();
		}
	}
}
