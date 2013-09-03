using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class ApiInfoFactoryTest
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
		public void Create_Should_Call_AssemblyProvider_GetExecutingAssembly_Once()
		{
			// Arrange
			var assemblyProviderMock = CreateAssemblyProvider();
			IApiVersionFactory factory = CreateFactory(assemblyProviderMock.Object);

			// Act
			factory.Create();
			factory.Create();

			// Assert
			assemblyProviderMock.Verify(p => p.GetExecutingAssembly(), Times.Once());
		}

		[TestMethod]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment_Once()
		{
			// Arrange
			var environmentProviderMock = CreateEnvironmentProvider();
			IApiVersionFactory factory = CreateFactory(environmentProvider: environmentProviderMock.Object);

			// Act
			factory.Create();
			factory.Create();

			// Assert
			environmentProviderMock.Verify(p => p.GetCurrentEnvironment(), Times.Once());
		}
		#endregion

		#region Helper methods
		private static IApiVersionFactory CreateFactory(IAssemblyProvider assemblyProvider = null, IEnvironmentProvider environmentProvider = null)
		{
			return new ApiVersionFactory(
				assemblyProvider ?? CreateAssemblyProvider().Object,
				environmentProvider ?? CreateEnvironmentProvider().Object);
		}

		private static Mock<IAssemblyProvider> CreateAssemblyProvider()
		{
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiInfoFactoryTest).Assembly);
			return providerMock;
		}

		private static Mock<IEnvironmentProvider> CreateEnvironmentProvider()
		{
			var providerMock = new Mock<IEnvironmentProvider>();
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(Server.Configuration.Environment.Test);
			return providerMock;
		}
		#endregion
	}
}