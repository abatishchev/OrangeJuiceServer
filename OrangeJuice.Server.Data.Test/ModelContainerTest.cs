using System.Data.Entity;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Data.Test
{
	[TestClass]
	public class ModelContainerTest
	{
		[TestMethod]
		public void Ctor_Should_Call_ConnectionStringProvider_GetDefaultConnectionString()
		{
			// Arrange
			var providerMock = new Mock<IConnectionStringProvider>();
			providerMock.Setup(p => p.GetDefaultConnectionString()).Returns("connectionString");

			// Act
			DbContext context = new ModelContext(providerMock.Object);

			// Assert
			providerMock.VerifyAll();
		}
	}
}