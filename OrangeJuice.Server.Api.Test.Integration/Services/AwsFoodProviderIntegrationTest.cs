using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Test.Integration.Services
{
	[TestClass]
	public class AwsFoodProviderIntegrationTest
	{
		[TestMethod]
		public async Task Lookup_Should_Return_Descriptor()
		{
			using (IUnityContainer container = UnityConfig.InitializeContainer())
			{
				// Arrange
				IFoodProvider provider = container.Resolve<IFoodProvider>("Aws");

				// Act
				FoodDescriptor descriptor = await provider.Lookup("0747599330971", BarcodeType.EAN.ToString());

				// Assert
				descriptor.Should().NotBeNull();
				descriptor.Id.Should().Be("B00HSQEETM");
				descriptor.Title.Should().Be("Ghirardelli Valentine's Chocolate Squares Premium Chocolate Assortment");
				descriptor.Brand.Should().Be("Ghirardelli");
			}
		}
	}
}