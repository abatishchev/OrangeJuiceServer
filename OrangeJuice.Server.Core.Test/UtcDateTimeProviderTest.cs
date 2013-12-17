using System;
using System.Text.RegularExpressions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test
{
	[TestClass]
	public class UtcDateTimeProviderTest
	{
		[TestMethod]
		public void FormatToUniversal_Should_Return_DateTime_In_Universal_Format()
		{
			// Arrange
			IDateTimeProvider dateTimeProvider = new UtcDateTimeProvider();

			// Act
			string output = dateTimeProvider.FormatToUniversal(DateTime.UtcNow);

			// Assert
			Regex.IsMatch(output, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$").Should().BeTrue(); // TODO: update with recent FA
		}
	}
}