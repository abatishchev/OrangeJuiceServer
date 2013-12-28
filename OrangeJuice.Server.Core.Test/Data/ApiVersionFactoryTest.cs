using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;

using Environment = OrangeJuice.Server.Configuration.Environment;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class ApiVersionFactoryTest
	{
		#region Create
		[TestMethod]
		public void Create_Should_Call_AssemblyProvider_GetExecutingAssembly()
		{
			// Arrange
			var assemblyProviderMock = CreateAssemblyProvider();
			var factory = CreateFactory(assemblyProvider: assemblyProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			assemblyProviderMock.Verify(p => p.GetExecutingAssembly(), Times.Once());
		}

		[TestMethod]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider();
			var factory = CreateFactory(environmentProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			environmentProviderMock.Verify(p => p.GetCurrentEnvironment(), Times.Once());
		}

		[TestMethod]
		public void Create_Should_Return_ApiVersion_Having_Key_When_Environment_Is_Local_Testing()
		{
			foreach (string environment in new[] { Environment.Local, Environment.Testing })
			{
				// Arrange
				var environmentProviderMock = CreateEnvironmentProvider(environment);
				var factory = CreateFactory(environmentProviderMock.Object);

				// Act
				ApiVersion apiVersion = factory.Create();

				// Assert
				apiVersion.Key.Should().HaveValue();
			}
		}

		[TestMethod]
		public void Create_Should_Return_ApiVersion_Not_Having_Key_When_Environment_Is_Development_Staging_Production()
		{
			foreach (string environment in new[] { Environment.Development, Environment.Staging, Environment.Production })
			{
				// Arrange
				var environmentProviderMock = CreateEnvironmentProvider(environment);
				var factory = CreateFactory(environmentProviderMock.Object);

				// Act
				ApiVersion apiVersion = factory.Create();

				// Assert
				apiVersion.Key.Should().NotHaveValue();
			}
		}

		[TestMethod]
		public void Create_Should_Should_Throw_Exception_When_EnvironmentProvider_GetCurrentEnvironment_Returns_Unsupported_Environment()
		{
			// Arrange
			const string environment = "anyEnvironment";

			var environmentProviderMock = CreateEnvironmentProvider(environment);
			var factory = CreateFactory(environmentProviderMock.Object);

			// Act
			Action action = () => factory.Create();

			// Assert
			action.ShouldThrow<NotSupportedException>();
		}
		#endregion

		#region Helper methods
		private static IFactory<ApiVersion> CreateFactory(IEnvironmentProvider environmentProvider = null, IAssemblyProvider assemblyProvider = null)
		{
			return new ApiVersionFactory(
				assemblyProvider ?? CreateAssemblyProvider().Object,
				environmentProvider ?? CreateEnvironmentProvider().Object);
		}

		private static Mock<IAssemblyProvider> CreateAssemblyProvider()
		{
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiVersionFactoryTest).Assembly);
			return providerMock;
		}

		private static Mock<IEnvironmentProvider> CreateEnvironmentProvider(string environment = null)
		{
			var providerMock = new Mock<IEnvironmentProvider>();
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment ?? Environment.Testing);
			return providerMock;
		}
		#endregion
	}
}