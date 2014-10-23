using System;

using Factory;
using FluentAssertions;
using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

using Xunit.Extensions;

namespace OrangeJuice.Server.Test.Data
{
	public class ApiVersionFactoryTest
	{
		#region Create
		[Theory]
		[InlineData(typeof(ApiVersionFactory))]
		[InlineData(typeof(FSharp.Data.ApiVersionFactory))]
		public void Create_Should_Call_AssemblyProvider_GetExecutingAssembly(Type type)
		{
			// Arrange
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiVersionFactoryTest).Assembly);

			var factory = CreateFactory(type, providerMock.Object);

			// Act
			factory.Create();

			// Assert
			providerMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(ApiVersionFactory))]
		[InlineData(typeof(FSharp.Data.ApiVersionFactory))]
		public void Create_Should_Return_AviVersion_Having_Environment_Returned_By_EnvironmentProvider_GetCurrentEnvironment(Type type)
		{
			// Arrange
			const string environment = EnvironmentName.Testing;

			var providerMock = CreateEnvironmentProvider(environment);

			var factory = CreateFactory(type, environmentProvider: providerMock);

			// Act
			ApiVersion apiVersion = factory.Create();

			// Assert
			apiVersion.Environment.Should().Be(environment);
		}
		#endregion

		#region Helper methods
		private static IFactory<ApiVersion> CreateFactory(Type type, IAssemblyProvider assemblyProvider = null, IEnvironmentProvider environmentProvider = null)
		{
			return (IFactory<ApiVersion>)Activator.CreateInstance(type,
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