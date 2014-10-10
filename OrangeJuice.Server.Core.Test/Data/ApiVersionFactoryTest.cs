using FluentAssertions;

using Xunit;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	public class ApiVersionFactoryTest
	{
		#region Create
		[Fact]
		public void Create_Should_Call_AssemblyProvider_GetExecutingAssembly()
		{
			// Arrange
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiVersionFactoryTest).Assembly);

			var factory = CreateFactory(providerMock.Object);

			// Act
			factory.Create();

			// Assert
			providerMock.Verify(p => p.GetExecutingAssembly(), Times.Once);
		}

		[Fact]
		public void Create_Should_Return_AviVersion_Having_Environment_Returned_By_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			const string environment = Environment.Testing;

			var providerMock = CreateEnvironmentProvider(environment);

			var factory = CreateFactory(environmentProvider: providerMock);

			// Act
			ApiVersion apiVersion = factory.Create();

			// Assert
			apiVersion.Environment.Should().Be(environment);
		}
		#endregion

		#region Helper methods
		private static Factory.IFactory<ApiVersion> CreateFactory(IAssemblyProvider assemblyProvider = null, IEnvironmentProvider environmentProvider = null)
		{
			return new ApiVersionFactory(
				assemblyProvider ?? CreateAssemblyProvider(),
				environmentProvider ?? CreateEnvironmentProvider());
		}

		private static IAssemblyProvider CreateAssemblyProvider()
		{
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiVersionFactoryTest).Assembly);
			return providerMock.Object;
		}

		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = null)
		{
			var providerMock = new Mock<IEnvironmentProvider>();
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return providerMock.Object;
		}
		#endregion
	}
}