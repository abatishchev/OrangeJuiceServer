using System;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Attributes;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Validation.Infrustructure;

namespace OrangeJuice.Server.Api.Test.Validation.Infrustructure
{
	[TestClass]
	public class AttributeValidatorFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Container_Is_Null()
		{
			// Arrange
			const IUnityContainer container = null;

			// Act
			Action action = () => new AttributeValidatorFactory(container);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("container");
		}

		[TestMethod]
		public void CreateInstance_Should_Throw_Exception_When_Type_Is_Null()
		{
			// Arrange
			const Type type = null;

			IUnityContainer container = new Mock<IUnityContainer>().Object;
			ValidatorFactoryBase factory = new AttributeValidatorFactory(container);

			// Act
			Action action = () => factory.CreateInstance(type);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("type");
		}

		[TestMethod]
		public void CreateInstance_Should_Pass_ValidatorAttribute_ValidatorType_To_Container_Resolve()
		{
			// Arrange
			var containerMock = CreateContainer();
			ValidatorFactoryBase factory = new AttributeValidatorFactory(containerMock.Object);

			// Act
			factory.CreateInstance(typeof(DecoratedClass));

			// Assert
			containerMock.Verify(c => c.Resolve(typeof(DecoratedClassValidator), null), Times.Once());
		}

		[TestMethod]
		public void CreateInstance_Should_Return_Validator_Resolved_By_Container()
		{
			// Arrange
			IValidator expected = new Mock<IValidator>().Object;
			Type type = typeof(DecoratedClass);

			var containerMock = CreateContainer(expected);
			ValidatorFactoryBase factory = new AttributeValidatorFactory(containerMock.Object);

			// Act
			object actual = factory.CreateInstance(type);

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void CreateInstance_Should_Return_Null_When_Type_Is_Not_Decorated_By_ValidatorAttribute()
		{
			// Arrange
			Type type = typeof(NotDecoratedClass);

			var containerMock = CreateContainer();
			var factory = new AttributeValidatorFactory(containerMock.Object);

			// Act
			object result = factory.CreateInstance(type);

			// Assert
			result.Should().BeNull();
		}
		#endregion

		#region Helper methods
		private static Mock<IUnityContainer> CreateContainer(IValidator validator = null)
		{
			var containerMock = new Mock<IUnityContainer>();
			containerMock.Setup(c => c.Resolve(It.IsAny<Type>(), null)).Returns(validator ?? new Mock<IValidator>().Object);
			return containerMock;
		}
		#endregion

		#region Inner classes
		private class NotDecoratedClass
		{
		}

		[Validator(typeof(DecoratedClassValidator))]
		private class DecoratedClass
		{
		}

		private class DecoratedClassValidator : AbstractValidator<DecoratedClass>
		{
		}
		#endregion
	}
}
