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
		public void Encode_Should_Execute_Pipeline()
		{
			// Arrange
			bool called = false;
			Action callback = () =>
				{
					called = true;
				};

			var pipelineMock = new Mock<IPipeline<string>>();
			pipelineMock.Setup(p => p.Execute(It.IsAny<string>()))
						.Returns(String.Empty)
						.Callback(callback);

			IUrlEncoder urlEncoder = new PercentUrlEncoder(pipelineMock.Object);

			// Act
			urlEncoder.Encode("anyUrl");

			// Assert
			pipelineMock.VerifyAll();
			called.Should().BeTrue();
		}
	}
}