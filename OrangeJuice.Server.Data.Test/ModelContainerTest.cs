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
		public void Ctor_Should_Call_ConfigurationProvider_Get()
		{
			// Arrange
			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue("sql:ConnectionString")).Returns("name=Test");

			// Act
			DbContext context = new ModelContext(providerMock.Object);

			// Assert
			providerMock.VerifyAll();
		}
	}
}