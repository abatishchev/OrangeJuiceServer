using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	[TestClass]
	public class AwsProductProviderIntegrationTest
	{
		[TestMethod]
		public async Task Lookup_Should_Return_ProductDescriptor()
		{
			using (IUnityContainer container = UnityConfig.InitializeContainer())
			{
				// Arrange
				IProductProvider provider = container.Resolve<IProductProvider>("Aws");

				// Act
				ProductDescriptor descriptor = await provider.SearchBarcode("0747599330971", BarcodeType.EAN);

				// Assert
				descriptor.Should().NotBeNull();
				descriptor.Id.Should().Be("B00HSQEETM");
				descriptor.Title.Should().Be("Ghirardelli Valentine's Chocolate Squares Premium Chocolate Assortment");
				descriptor.Brand.Should().Be("Ghirardelli");
			}
		}
	}
}