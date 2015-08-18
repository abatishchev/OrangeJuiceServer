using Ab.Configuration;

using FluentAssertions;
using Moq;
using Xunit;

namespace OrangeJuice.Server.Data.Test
{
	public class ModelContextTest
	{
		[Fact]
		public void Ctor_Should_Call_ConnectionStringProvider_GetDefaultConnectionString()
		{
			// Arrange
			const string connectionString = "Data Source=connectionString";

			var providerMock = new Mock<IConnectionStringProvider>();
			providerMock.Setup(p => p.GetDefaultConnectionString()).Returns(connectionString);

			// Act
			var context = new ModelContext(providerMock.Object);

			// Assert
			context.Database.Connection.ConnectionString.Should().Be(connectionString);
			providerMock.VerifyAll();
		}
	}
}