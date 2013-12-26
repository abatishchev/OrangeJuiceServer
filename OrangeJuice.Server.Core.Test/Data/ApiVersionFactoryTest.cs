using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class ApiVersionFactoryTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AssemblyProvider_Is_Null()
		{
			// Arange
			const IAssemblyProvider assemblyProvider = null;
			const IEnvironmentProvider environmentProvider = null;

			// Act
			Action action = () => new ApiVersionFactory(assemblyProvider, environmentProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("assemblyProvider");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_EnvironmentProvider_Is_Null()
		{
			// Arange
			IAssemblyProvider assemblyProvider = CreateAssemblyProvider().Object;
			const IEnvironmentProvider environmentProvider = null;

			// Act
			Action action = () => new ApiVersionFactory(assemblyProvider, environmentProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("environmentProvider");
		}
		#endregion

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
		public void Create_Should_Return_ApiVersion_Having_Key_When_Environment_Is_Local_Testing_Development()
		{
			foreach (string environment in new[]
										   {
												Server.Configuration.Environment.Local,
												Server.Configuration.Environment.Testing,
												Server.Configuration.Environment.Development
										   })
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
		public void Create_Should_Return_ApiVersion_Not_Having_Key_When_Environment_Is_Staging_Production()
		{
			foreach (string environment in new[]
										   {
												Server.Configuration.Environment.Staging,
												Server.Configuration.Environment.Production
										   })
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
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment ?? Server.Configuration.Environment.Testing);
			return providerMock;
		}
		#endregion
	}
}