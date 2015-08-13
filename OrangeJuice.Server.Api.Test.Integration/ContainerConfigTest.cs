using System;
using System.Web.Http;

using FluentAssertions;
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
			Action action = () => container.Verify(VerificationOption.VerifyOnly);

			// Asssert
			action.ShouldNotThrow();
		}

		[Fact]
		public void OwinContainer_Verify_Should_Not_Throw_Exception()
		{
			// Arrange
			var container = ContainerConfig.CreateOwinContainer();

			// Act
			Action action = () => container.Verify(VerificationOption.VerifyOnly);

			// Asssert
			action.ShouldNotThrow();
		}
	}
}
