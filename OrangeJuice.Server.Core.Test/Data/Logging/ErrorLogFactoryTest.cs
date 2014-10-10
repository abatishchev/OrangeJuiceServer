using System;
using System.Collections.Generic;
using System.Linq;
using Elmah;
using Factory;
using FluentAssertions;
using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Logging;

using Xunit.Extensions;

namespace OrangeJuice.Server.Test.Data.Logging
{
	public class ErrorLogFactoryTest
	{
		#region Tests
		[Theory]
		[InlineData(typeof(ErrorLogFactory))]
		[InlineData(typeof(FSharp.Data.Logging.ErrorLogFactory))]
		public void Create_Should_Return_EntityErrorLog_When_Environment_Is_Production(Type type)
		{
			// Arrange
			var environmentProvider = CreateEnvironmentProvider(EnvironmentName.Production);
			var connectionStringProvider = CreateConnectionStringProvider();

			var factory = CreateFactory(type, environmentProvider, connectionStringProvider);

			// Act
			var errorLog = factory.Create();

			// Assert
			errorLog.Should().BeOfType<Elmah.Contrib.EntityFramework.EntityErrorLog>();
		}

		[Theory]
		[InlineData(typeof(ErrorLogFactory))]
		[InlineData(typeof(FSharp.Data.Logging.ErrorLogFactory))]
		public void Create_Should_Return_TraceErrorLog_When_Environment_Is_Not_Production(Type type)
		{
			foreach (string environment in GetAllEnvironments().Except(EnvironmentName.Production))
			{
				// Arrange
				var environmentProvider = CreateEnvironmentProvider(environment);
				var connectionStringProvider = Mock.Of<IConnectionStringProvider>();

				var factory = CreateFactory(type, environmentProvider, connectionStringProvider);

				// Act
				var errorLog = factory.Create();

				// Assert
				errorLog.Should().BeOfType<TraceErrorLog>();
			}
		}
		#endregion

		#region Helper methods
		private static IFactory<ErrorLog> CreateFactory(Type type, IEnvironmentProvider environmentProvider, IConnectionStringProvider connectionStringProvider)
		{
			return (IFactory<ErrorLog>)Activator.CreateInstance(type,
				environmentProvider, connectionStringProvider);
		}

		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = EnvironmentName.Testing)
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