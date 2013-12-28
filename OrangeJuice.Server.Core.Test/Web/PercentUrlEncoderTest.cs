using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Web
{
	[TestClass]
	public class PercentUrlEncoderTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_EncodingPipeline_Is_Null()
		{
			// Arange
			const IPipeline<string> encodingPipeline = null;

			// Act
			Action action = () => new PercentUrlEncoder(encodingPipeline);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("encodingPipeline");
		}

		[TestMethod]
		public void Encode_Should_Execute_EncodingPipeline()
		{
			// Arrange
			bool called = false;
			Func<string, string> operation = s =>
				{
					called = true;
					return s;
				};

			var pipelineMock = new Mock<IPipeline<string>>();
			pipelineMock.Setup(p => p.GetOperations()).Returns(new[] { operation });

			IUrlEncoder urlEncoder = new PercentUrlEncoder(pipelineMock.Object);

			// Act
			urlEncoder.Encode("anyUrl");

			// Assert
			called.Should().BeTrue();
		}
	}
}