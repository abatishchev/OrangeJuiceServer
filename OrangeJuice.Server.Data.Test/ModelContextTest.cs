﻿using Moq;

using OrangeJuice.Server.Configuration;

using Xunit;

namespace OrangeJuice.Server.Data.Test
{
    public class ModelContextTest
    {
        [Fact]
        public void Ctor_Should_Call_ConnectionStringProvider_GetDefaultConnectionString()
        {
            // Arrange
            var providerMock = new Mock<IConnectionStringProvider>();
            providerMock.Setup(p => p.GetDefaultConnectionString()).Returns("connectionString");

            // Act
            var context = new ModelContext(providerMock.Object);

            // Assert
            providerMock.VerifyAll();
        }
    }
}