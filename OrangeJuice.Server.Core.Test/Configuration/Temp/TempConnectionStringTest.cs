using System.Configuration;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	[TestClass]
	public class TempConnectionStringTest
	{
		[TestMethod]
		public void Ctor_Should_Add_ConnectionString_And_Dipose_Should_Remove()
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
			ConfigurationManager.ConnectionStrings[name].Should().BeNull();
		}
	}
}