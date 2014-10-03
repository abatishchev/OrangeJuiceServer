using System.Linq;

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
		public void Search_Should_Return_ProductDescriptor()
		{
			using (IUnityContainer container = ContainerConfig.CreateContainer())
			{
				// Arrange
				IAwsProductProvider provider = container.Resolve<IAwsProductProvider>();

				// Act
				ProductDescriptor descriptor = provider.Search("0747599330971", BarcodeType.EAN).FirstOrDefault();

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