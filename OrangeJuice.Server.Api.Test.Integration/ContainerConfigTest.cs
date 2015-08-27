using System.Web.Http;
using SimpleInjector;
using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration
{
	public class ContainerConfigTest
	{
		[Fact]
		public void WebApiContainer_Verify_Should_Not_Throw_Exception()
		{
			// Arrange
			var container = ContainerConfig.CreateWebApiContainer();
			WebApiConfig.Configure(new HttpConfiguration(), container);

			// Act
			// Asssert
			container.Verify(VerificationOption.VerifyOnly);
		}

		[Fact]
		public void OwinContainer_Verify_Should_Not_Throw_Exception()
		{
			// Arrange
			var container = ContainerConfig.CreateOwinContainer();

			// Act
			// Asssert
			container.Verify();
		}
	}
}