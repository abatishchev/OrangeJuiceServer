using System;
using System.Collections;
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
			const string name = "Test";
			const string expected = "Data Source=localhost;Initial Catalog=Test;Integrated Security=True;";
			string entityConnectionString = String.Format("metadata=res://*/;provider=System.Data.SqlClient;provider connection string=&quot;{0}&quot;", expected);

			using (new Configuration.TempConnectionString(name, entityConnectionString))
			{
				IDictionary config = new Dictionary<string, string> { { EntityErrorLog.ConnectionStringNameKey, name } };
				EntityErrorLog errorLog = new EntityErrorLog(config);

				// Act
				string actual = errorLog.ConnectionString;

				// Assert
				actual.Should().Be(expected);
			}
		}
	}
}