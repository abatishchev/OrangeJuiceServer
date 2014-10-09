using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.FSharp.Data.Logging;

namespace OrangeJuice.Server.FSharp.Test.Data.Logging
{
	[TestClass]
	public class ErrorLogFactoryFSharpTest
	{
		#region Test methods
		[TestMethod]
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

		[TestMethod]
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