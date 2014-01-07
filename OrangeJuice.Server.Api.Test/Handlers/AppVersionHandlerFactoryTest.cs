using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class AppVersionHandlerFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider();
			var factory = new AppVersionHandlerFactory(environmentProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			environmentProviderMock.Verify(p => p.GetCurrentEnvironment(), Times.Once());
		}

		[TestMethod]
		public void Create_Should_Return_EmptyAppKeyHandler_When_Environment_Is_Local()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider(Environment.Local);
			var factory = new AppVersionHandlerFactory(environmentProviderMock.Object);

			// Act
			AppVersionHandler handler = factory.Create();

			// Assert
			handler.Should().BeOfType<EmptyAppVersionHandler>();
		}

		[TestMethod]
		public void Create_Should_Return_QueryAppKeyHandler_When_Environment_Is_Development_Staging()
		{
			foreach (string environment in new[]
										   {
												Environment.Development,
												Environment.Staging
										   })
			{
				// Arrange
				var environmentProviderMock = CreateEnvironmentProvider(environment);
				var factory = new AppVersionHandlerFactory(environmentProviderMock.Object);

				// Act
				AppVersionHandler handler = factory.Create();

				// Assert
				handler.Should().BeOfType<QueryAppVersionHandler>();
			}
		}

		[TestMethod]
		public void Create_Should_Return_HeaderAppKeyHandler_When_Environment_Is_Production()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider(Environment.Production);
			var factory = new AppVersionHandlerFactory(environmentProviderMock.Object);

			// Act
			AppVersionHandler handler = factory.Create();

			// Assert
			handler.Should().BeOfType<HeaderAppVersionHandler>();
		}
		#endregion

		#region Helper methods
		private static Mock<IEnvironmentProvider> CreateEnvironmentProvider(string environment = Environment.Testing)
		{
			var environmentProviderMock = new Mock<IEnvironmentProvider>();
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return environmentProviderMock;
		}
		#endregion
	}
}