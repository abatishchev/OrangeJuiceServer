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
		public void Encode_Should_Execute_EncodingPipeline()
		{
			// Arrange
			bool called = false;
			Func<string, string> operation = s =>
				{
					called = true;
					return s;
				};

			var pipelineMock = new Mock<AggregatePipeline<string>> { CallBase = true };
			pipelineMock.Setup(p => p.GetOperations()).Returns(new[] { operation });

			IUrlEncoder urlEncoder = new PercentUrlEncoder(pipelineMock.Object);

			// Act
			urlEncoder.Encode("anyUrl");

			// Assert
			called.Should().BeTrue();
		}
	}
}