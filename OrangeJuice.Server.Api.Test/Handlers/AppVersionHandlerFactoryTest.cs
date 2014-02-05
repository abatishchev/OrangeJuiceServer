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
			IEnvironmentProvider provider = CreateEnvironmentProvider();
			AppVersionHandlerFactory factory = new AppVersionHandlerFactory(provider);

			// Act
			factory.Create();

			// Assert
			Mock.Get(provider).Verify(p => p.GetCurrentEnvironment(), Times.Once);
		}

		[TestMethod]
		public void Create_Should_Return_EmptyAppKeyHandler_When_Environment_Is_Local()
		{
			// Arrange
			IEnvironmentProvider provider = CreateEnvironmentProvider(Environment.Local);
			AppVersionHandlerFactory factory = new AppVersionHandlerFactory(provider);

			// Act
			AppVersionHandler handler = factory.Create();

			// Assert
			handler.Should().BeOfType<EmptyAppVersionHandler>();
		}

		[TestMethod]
		public void Create_Should_Return_QueryAppKeyHandler_When_Environment_Is_Development_Staging()
		{
			foreach (string environment in new[] { Environment.Development, Environment.Staging })
			{
				// Arrange
				IEnvironmentProvider provider = CreateEnvironmentProvider(environment);
				AppVersionHandlerFactory factory = new AppVersionHandlerFactory(provider);

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
			IEnvironmentProvider provider = CreateEnvironmentProvider(Environment.Production);
			AppVersionHandlerFactory factory = new AppVersionHandlerFactory(provider);

			// Act
			AppVersionHandler handler = factory.Create();

			// Assert
			handler.Should().BeOfType<HeaderAppVersionHandler>();
		}
		#endregion

		#region Helper methods
		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = Environment.Testing)
		{
			var providerMock = new Mock<IEnvironmentProvider>();
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return providerMock.Object;
		}
		#endregion
	}
}