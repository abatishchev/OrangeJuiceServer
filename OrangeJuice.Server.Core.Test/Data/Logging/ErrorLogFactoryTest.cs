using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Logging;

namespace OrangeJuice.Server.Test.Data.Logging
{
	public class ErrorLogFactoryTest
	{
		#region Test methods
		[Fact]
		public void Create_Should_Return_EntityErrorLog_When_Environment_Is_Production()
		{
			// Arrange
			var environmentProvider = CreateEnvironmentProvider(Environment.Production);
			var connectionStringProvider = CreateConnectionStringProvider();

			var factory = new ErrorLogFactory(environmentProvider, connectionStringProvider);

			// Act
			var errorLog = factory.Create();

			// Assert
			errorLog.Should().BeOfType<Elmah.Contrib.EntityFramework.EntityErrorLog>();
		}

		[Fact]
		public void Create_Should_Return_TraceErrorLog_When_Environment_Is_Not_Production()
		{
			foreach (string environment in GetAllEnvironments().Except(Environment.Production))
			{
				// Arrange
				var environmentProvider = CreateEnvironmentProvider(environment);
				var connectionStringProvider = Mock.Of<IConnectionStringProvider>();

				var factory = new ErrorLogFactory(environmentProvider, connectionStringProvider);

				// Act
				var errorLog = factory.Create();

				// Assert
				errorLog.Should().BeOfType<TraceErrorLog>();
			}
		}
		#endregion

		#region Helper methods
		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = Environment.Testing)
		{
			var providerMock = new Mock<IEnvironmentProvider>();
			providerMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return providerMock.Object;
		}

		private static IEnumerable<string> GetAllEnvironments()
		{
			return typeof(Environment).GetFields().Select(f => (string)f.GetValue(null));
		}

		private static IConnectionStringProvider CreateConnectionStringProvider()
		{
			var providerMock = new Mock<IConnectionStringProvider>();
			providerMock.Setup(p => p.GetDefaultConnectionString()).Returns("connectionString");
			return providerMock.Object;
		}
		#endregion
	}
}