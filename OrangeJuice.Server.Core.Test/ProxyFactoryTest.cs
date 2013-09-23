using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test
{
	[TestClass]
	public class ProxyFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Func_Is_Null()
		{
			// Arrange
			const Func<object> func = null;

			// Act
			Action action = () => new ProxyFactory<object>(func);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("func");
		}

		[TestMethod]
		public void Create_Should_Call_Func()
		{
			// Arrange
			bool called = false;
			Func<object> func = () =>
				{
					called = true;
					return new object();
				};

			IFactory<object> factory = new ProxyFactory<object>(func);

			// Act
			factory.Create();

			// Assert
			called.Should().BeTrue();
		}

		[TestMethod]
		public void Create_Should_Return_Object_Returned_By_Func()
		{
			// Arrange
			object expected = new object();
			Func<object> func = () => expected;

			IFactory<object> factory = new ProxyFactory<object>(func);

			// Act
			object actual = factory.Create();

			// Assert
			actual.Should().Be(expected);
		}
		#endregion
	}
}