using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Diagnostics;
using OrangeJuice.Server.Test.Configuration.Temp;

namespace OrangeJuice.Server.Api.Test.Diagnostics
{
	[TestClass]
	public class EntityErrorLogTest
	{
		[TestMethod]
		public void ConnectionString_Should_Return_Provider_Part_From_Connection_String()
		{
			// Arrange
			const string name = "Test";
			const string expected = "Data Source=localhost;Initial Catalog=Test;Integrated Security=True";
			string entityConnectionString = String.Format("metadata=res://*/;provider=System.Data.SqlClient;provider connection string=\"{0}\"", expected);

			using (new TempConnectionString(name, entityConnectionString))
			{
				var config = new Dictionary<string, string> { { EntityErrorLog.ConnectionStringNameKey, name } };
				EntityErrorLog errorLog = new EntityErrorLog(config);

				// Act
				string actual = errorLog.ConnectionString;

				// Assert
				actual.Should().Be(expected);
			}
		}
	}
}