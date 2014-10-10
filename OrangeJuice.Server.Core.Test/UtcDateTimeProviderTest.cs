using System;

using FluentAssertions;

using Xunit;

namespace OrangeJuice.Server.Test
{
	public class UtcDateTimeProviderTest
	{
		[Fact]
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