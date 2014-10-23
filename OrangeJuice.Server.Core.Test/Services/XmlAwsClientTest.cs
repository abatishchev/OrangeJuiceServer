using System;
using System.Linq;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Test.Services
{
	public class XmlAwsClientTest
	{
	    [Fact]
	    public void GetItems_Should_Execute_Pipeline()
	    {
	        // Arrange
	        bool called = false;
	        Action callback = () =>
	                          {
	                              called = true;
	                          };

	        var pipelineMock = new Mock<IPipeline>();
	        pipelineMock.Setup(p => p.Execute(It.IsAny<object>()))
	                    .Returns(Enumerable.Empty<ProductDescriptor>())
	                    .Callback(callback);

	        IAwsClient client = new XmlAwsClient(pipelineMock.Object);

	        // Act
	        client.GetItems(new ProductDescriptorSearchCriteria());

	        // Assert
	        pipelineMock.VerifyAll();
	        called.Should().BeTrue();
	    }
	}
}