using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Moq;
using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Validation;
using Xunit;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	public class AppVersionValidatorFactoryTest
	{
		#region Test methods
		[Fact]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			var provider = CreateEnvironmentProvider();
			var factory = new AcceptHeaderValidatorFactory(provider);

			// Act
			factory.Create();

			// Assert
			Mock.Get(provider).Verify(p => p.GetCurrentEnvironment(), Times.Once);
		}

		[Fact]
		public void Create_Should_Return_EmptyValidator_When_Environment_Is_Local()
		{
			// Arrange
			var provider = CreateEnvironmentProvider(EnvironmentName.Local);
			var factory = new AcceptHeaderValidatorFactory(provider);

			// Act
			var handler = factory.Create();

			// Assert
			handler.Should().BeOfType<EmptyValidator<HttpRequestMessage>>();
		}

		[Fact]
		public void Create_Should_Return_AcceptHeaderValidator_When_Environment_Is_Not_Local()
		{
			foreach (string environment in GetAllEnvironments().Except(EnvironmentName.Local))
			{
				// Arrange
				var provider = CreateEnvironmentProvider(environment);
				var factory = new AcceptHeaderValidatorFactory(provider);

				// Act
				var handler = factory.Create();

				// Assert
				handler.Should().BeOfType<AcceptHeaderValidator>();
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