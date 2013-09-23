using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Test
{
	[TestClass]
	public class ProxyFactoryBaseTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Func_Is_Null()
		{
			// Arrange
			const Func<Stub> func = null;

			// Act
			Action action = () => new ProxyFactoryStub(func);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("func");
		}

		[TestMethod]
		public void Create_Should_Call_Func()
		{
			// Arrange
			bool called = false;
			Func<Stub> func = () =>
				{
					called = true;
					return new Stub();
				};

			IFactory<Stub> factory = new ProxyFactoryStub(func);

			// Act
			factory.Create();

			// Assert
			called.Should().BeTrue();
		}

		[TestMethod]
		public void Create_Should_Return_Object_Returned_By_Func()
		{
			// Arrange
			Stub expected = new Stub();
			Func<Stub> func = () => expected;

			IFactory<Stub> factory = new ProxyFactoryStub(func);

			// Act
			Stub actual = factory.Create();

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper classes
		private class Stub
		{
		}

		private class ProxyFactoryStub : ProxyFactoryBase<Stub>
		{
			public ProxyFactoryStub(Func<Stub> func)
				: base(func)
			{
			}
		}
		#endregion
	}
}