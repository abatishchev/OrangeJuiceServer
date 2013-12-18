using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class AppKeyHandlerFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_EnvironmentProvider_Is_Null()
		{
			// Arrange
			const IEnvironmentProvider environmentProvider = null;

			// Act
			Action action = () => new AppKeyHandlerFactory(environmentProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("environmentProvider");
		}

		[TestMethod]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider();
			var factory = new AppKeyHandlerFactory(environmentProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			environmentProviderMock.Verify(p => p.GetCurrentEnvironment(), Times.Once());
		}

		[TestMethod]
		public void Create_Should_Return_EmptyAppKeyHandler_When_Environment_Is_Local()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider(Configuration.Environment.Local);
			var factory = new AppKeyHandlerFactory(environmentProviderMock.Object);

			// Act
			AppKeyHandlerBase handler = factory.Create();

			// Assert
			handler.Should().BeOfType<EmptyAppKeyHandler>();
		}

		[TestMethod]
		public void Create_Should_Return_QueryAppKeyHandler_When_Environment_Is_Development_Staging()
		{
			foreach (string environment in new[]
										   {
												Configuration.Environment.Development,
												Configuration.Environment.Staging
										   })
			{
				// Arrange
				var environmentProviderMock = CreateEnvironmentProvider(environment);
				var factory = new AppKeyHandlerFactory(environmentProviderMock.Object);

				// Act
				AppKeyHandlerBase handler = factory.Create();

				// Assert
				handler.Should().BeOfType<QueryAppKeyHandler>();
			}
		}

		[TestMethod]
		public void Create_Should_Return_HeaderAppKeyHandler_When_Environment_Is_Production()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider(Configuration.Environment.Production);
			var factory = new AppKeyHandlerFactory(environmentProviderMock.Object);

			// Act
			AppKeyHandlerBase handler = factory.Create();

			// Assert
			handler.Should().BeOfType<HeaderAppKeyHandler>();
		}
		#endregion

		#region Helper methods
		private static Mock<IEnvironmentProvider> CreateEnvironmentProvider(string environment = Configuration.Environment.Testing)
		{
			var environmentProviderMock = new Mock<IEnvironmentProvider>(MockBehavior.Strict);
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return environmentProviderMock;
		}
		#endregion
	}
}