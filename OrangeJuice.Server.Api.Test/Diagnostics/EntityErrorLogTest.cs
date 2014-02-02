using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Diagnostics;

namespace OrangeJuice.Server.Api.Test.Diagnostics
{
	[TestClass]
	public class EntityErrorLogTest
	{
		[TestMethod]
		public void ConnectionString_Should_Return_Provider_Part_From_Connection_String()
		{
			// Arrange
			var config = new Dictionary<string, string> { { "connectionStringName", "Test" } };
			EntityErrorLog errorLog = new EntityErrorLog(config);

			// Act
			string connectionString = errorLog.ConnectionString;

			// Assert
			connectionString.Should().Be("Value");
		}
	}
}