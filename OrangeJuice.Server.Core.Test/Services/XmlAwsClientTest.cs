using System;
using System.Linq;


using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlAwsClientTest
	{
	    [TestMethod]
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