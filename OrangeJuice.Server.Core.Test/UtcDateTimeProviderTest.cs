using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test
{
	[TestClass]
	public class UtcDateTimeProviderTest
	{
		[TestMethod]
		public void Format_Should_Return_DateTime_In_Universal_Format()
		{
			// Arrange
			IDateTimeProvider dateTimeProvider = new UtcDateTimeProvider();

			// Act
			string output = dateTimeProvider.Format(DateTime.UtcNow);

			// Assert
			output.Should().MatchRegex(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$");
		}
	}
}