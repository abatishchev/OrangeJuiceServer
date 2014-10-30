using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	public class AwsProductProviderIntegrationTest
	{
		[Fact]
		public async Task Search_Should_Return_ProductDescriptor()
		{
			using (IUnityContainer container = ContainerConfig.CreateWebApiContainer())
			{
				// Arrange
				IAwsProductProvider provider = container.Resolve<IAwsProductProvider>();

				// Act
				ProductDescriptor descriptor = (await provider.Search("0747599330971", BarcodeType.EAN)).FirstOrDefault();

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
}