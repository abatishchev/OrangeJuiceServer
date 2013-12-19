using System.Configuration;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	[TestClass]
	public class TempAppSettingsTest
	{
		[TestMethod]
		public void Ctor_Should_Add_AppSettings_And_Dipose_Should_Remove()
		{
			// Arrange
			const string name = "TestName";
			const string expected = "TestValue";

			// Act
			ConfigurationManager.AppSettings[name].Should().BeNull();
			using (new TempAppSettings(name, expected))
			{
				// Assert
				string actual = ConfigurationManager.AppSettings[name];
				actual.Should().Be(expected);
			}
			ConfigurationManager.AppSettings[name].Should().BeNull();
		}
	}
}