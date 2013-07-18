using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Api.Test
{
    [TestClass]
    public class TestContextTest
    {
        [TestMethod]
        public void Ctor_Should_Throw_Exception_When_TestAction_IsNull()
        {
            // Arrange
            const Action testAction = null;

            // Act
            Action action = () => new TestContext(testAction);

            // Assert
            action.ShouldThrow<ArgumentException>()
                  .And.ParamName.Should().Be("testAction");
        }

        [TestMethod]
        public void Ctor_Should_Not_Throw_Exception_When_DisposeAction_IsNull()
        {
            // Arrange
            Action testAction = () => { };
            Action diposeAction = null;

            // Act
            Action action = () => new TestContext(testAction, diposeAction);

            // Assert
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void Ctor_Should_Call_TestAction()
        {
            // Arrange
            bool called = false;
            Action testAction = () => called = true;

            // Act
            new TestContext(testAction);

            // Assert
            called.Should().BeTrue();
        }

        [TestMethod]
        public void Dispose_Should_Call_DisposeAction()
        {
            // Arrange
            Action testAction = () => { };
            bool called = false;
            Action diposeAction = () => called = true;
            IDisposable testContext = new TestContext(testAction, diposeAction);

            // Act
            testContext.Dispose();

            // Assert
            called.Should().BeTrue();
        }
    }
}