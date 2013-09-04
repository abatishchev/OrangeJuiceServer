using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsClientFactoryTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsOptions_Is_Null()
		{
			// Arange
			const AwsOptions awsOptions = null;
			const IUrlEncoder urlEncoder = null;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new AwsClientFactory(awsOptions, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsOptions");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_UrlEncoder_Is_Null()
		{
			// Arange
			AwsOptions awsOptions = new AwsOptions();
			const IUrlEncoder urlEncoder = null;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new AwsClientFactory(awsOptions, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("urlEncoder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_DateTimeProvider_Is_Null()
		{
			// Arange
			AwsOptions awsOptions = new AwsOptions();
			IUrlEncoder urlEncoder = new Mock<IUrlEncoder>().Object;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new AwsClientFactory(awsOptions, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("dateTimeProvider");
		}

		[TestMethod]
		public void Create_Should_Return_XmlAwsClient()
		{
			// Arrange
			IAwsClientFactory factory = new AwsClientFactory(
				new AwsOptions
				{
					AccessKey = "anyKey",
					AssociateTag = "anyTag",
					SecretKey = "anyKey"
				},
				new Mock<IUrlEncoder>().Object,
				new Mock<IDateTimeProvider>().Object);

			// Act
			IAwsClient client = factory.Create();

			// Assert
			client.Should().BeOfType<AwsClient>();
		}
	}
}