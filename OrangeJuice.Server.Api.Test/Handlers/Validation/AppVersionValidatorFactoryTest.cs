using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using OrangeJuice.Server.Api.Handlers.Validation;
using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Test.Handlers.Validation
{
	[TestClass]
	public class AppVersionValidatorFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			var provider = CreateEnvironmentProvider();
			var factory = new AppVersionValidatorFactory(provider);

			// Act
			factory.Create();

			// Assert
			Mock.Get(provider).Verify(p => p.GetCurrentEnvironment(), Times.Once);
		}

		[TestMethod]
		public void Create_Should_Return_EmptyAppKeyHandler_When_Environment_Is_Local()
		{
			// Arrange
			var provider = CreateEnvironmentProvider(EnvironmentName.Local);
			var factory = new AppVersionValidatorFactory(provider);

			// Act
			var handler = factory.Create();

			// Assert
			handler.Should().BeOfType<EmptyAppVersionValidator>();
		}

		[TestMethod]
		public void Create_Should_Return_HeaderAppKeyHandler_When_Environment_Is_Not_Local()
		{
			foreach (string environment in GetAllEnvironments().Except(EnvironmentName.Local))
			{
				// Arrange
				var provider = CreateEnvironmentProvider(environment);
				var factory = new AppVersionValidatorFactory(provider);

				// Act
				var handler = factory.Create();

				// Assert
				handler.Should().BeOfType<HeaderAppVersionValidator>();
			}
		}
		#endregion

		#region Helper methods
		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = EnvironmentName.Testing)
		{
			var providerMock = new Mock<IEnvironmentProvider>();
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return providerMock.Object;
		}

		private static IEnumerable<string> GetAllEnvironments()
		{
			return typeof(EnvironmentName).GetFields().Select(f => (string)f.GetValue(null));
		}
		#endregion
	}
}