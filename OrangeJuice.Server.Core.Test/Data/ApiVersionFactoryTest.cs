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
		#region Create
		[TestMethod]
		public void Create_Should_Call_AssemblyProvider_GetExecutingAssembly()
		{
			// Arrange
			var assemblyProviderMock = CreateAssemblyProvider();
			var factory = CreateFactory(assemblyProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			assemblyProviderMock.Verify(p => p.GetExecutingAssembly(), Times.Once);
		}

		[TestMethod]
		public void Create_Should_Return_AviVersion_Having_Environment_Returned_By_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			const string environment = Environment.Testing;

			var environmentProviderMock = new Mock<IEnvironmentProvider>();
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);

			var factory = CreateFactory(environmentProvider: environmentProviderMock.Object);

			// Act
			ApiVersion apiVersion = factory.Create();

			// Assert
			apiVersion.Environment.Should().Be(environment);
		}
		#endregion

		#region Helper methods
		private static IFactory<ApiVersion> CreateFactory(IAssemblyProvider assemblyProvider = null, IEnvironmentProvider environmentProvider = null)
		{
			return new ApiVersionFactory(
				assemblyProvider ?? CreateAssemblyProvider().Object,
				environmentProvider ?? Mock.Of<IEnvironmentProvider>());
		}

		private static Mock<IAssemblyProvider> CreateAssemblyProvider()
		{
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiVersionFactoryTest).Assembly);
			return providerMock;
		}
		#endregion
	}
}