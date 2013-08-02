using System.Configuration;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	[TestClass]
	public class TempConnectionStringTest
	{
		[TestMethod]
		public void Ctor_Should_Add_Connection_String_When_It_Does_Not_Exist()
		{
			// Arrange
			const string name = "TestName";
			const string expected = "TestValue";

			// Act
			ConfigurationManager.ConnectionStrings[name].Should().BeNull();
			using (new TempConnectionString(name, expected))
			{
				// Assert
				string actual = ConfigurationManager.ConnectionStrings[name].ConnectionString;
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void Dispose_Should_Remove_Connection_String_When_It_Does_Not_Exist()
		{
			// Arrange
			const string name = "TestName";
			const string expected = "TestValue";
			TempConnectionString tempConnectionString = new TempConnectionString(name, expected);

			// Act
			ConfigurationManager.ConnectionStrings[name].Should().NotBeNull();
			tempConnectionString.Dispose();

			// Assert
			ConfigurationManager.ConnectionStrings[name].Should().BeNull();
		}
	}
}