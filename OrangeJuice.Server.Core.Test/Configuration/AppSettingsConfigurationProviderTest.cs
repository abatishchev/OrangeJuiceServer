using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Test.Configuration.Temp;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class AppSettingsConfigurationProviderTest
	{
		[TestMethod]
		public void GetValue_Should_Return_Value_From_AppSettings()
		{
			// Arrange
			const string name = "TestKey1";
			const string expected = "TestValue";
			IConfigurationProvider configurationProvider = new AppSettingsConfigurationProvider();

			// Act
			using (new TempAppSettings(name, expected))
			{
				string actual = configurationProvider.GetValue(name);

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public void GetValue_Should_Return_Null_When_AppSettings_Does_Not_Contain_Key_Specified()
		{
			// Arrange
			const string name = "TestKey2";
			IConfigurationProvider configurationProvider = new AppSettingsConfigurationProvider();

			// Act
			string actual = configurationProvider.GetValue(name);

			// Assert
			actual.Should().BeNull();
		}
	}
}