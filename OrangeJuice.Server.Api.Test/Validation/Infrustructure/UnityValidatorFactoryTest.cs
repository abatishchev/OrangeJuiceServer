using System;

using FluentAssertions;

using FluentValidation;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Validation.Infrustructure;

namespace OrangeJuice.Server.Api.Test.Validation.Infrustructure
{
	[TestClass]
	public class UnityValidatorFactoryTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Container_Is_Null()
		{
			// Arrange
			const IUnityContainer container = null;

			// Act
			Action action = () => new UnityValidatorFactory(container);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("container");
		}

		[TestMethod]
		public void CreateInstance_Should_Throw_Exception_When_Type_Is_Null()
		{
			// Arrange
			IUnityContainer container = new Mock<IUnityContainer>().Object;
			ValidatorFactoryBase factory = new UnityValidatorFactory(container);
			const Type type = null;

			// Act
			Action action = () => factory.CreateInstance(type);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("type");
		}

		[TestMethod]
		public void CreateInstance_Should_Pass_Type_To_Container_Resolve()
		{
			// Arrange
			Type type = typeof(object);

			var containerMock = new Mock<IUnityContainer>(MockBehavior.Strict);
			containerMock.Setup(c => c.Resolve(type, null)).Returns(null);
			ValidatorFactoryBase factory = new UnityValidatorFactory(containerMock.Object);

			// Act
			factory.CreateInstance(type);

			// Assert
			containerMock.Verify(c => c.Resolve(type, null), Times.Once());
		}

		[TestMethod]
		public void CreateInstance_Should_Return_Type_Resolved_By_Container()
		{
			// Arrange
			IValidator expected = new Mock<IValidator>().Object;
			Type type = typeof(object);

			var containerMock = new Mock<IUnityContainer>(MockBehavior.Strict);
			containerMock.Setup(c => c.Resolve(type, null)).Returns(expected);
			ValidatorFactoryBase factory = new UnityValidatorFactory(containerMock.Object);

			// Act
			object actual = factory.CreateInstance(type);

			// Assert
			actual.Should().Be(expected);
		}
	}
}
