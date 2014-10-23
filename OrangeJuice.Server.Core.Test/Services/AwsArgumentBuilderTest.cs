using System;

using FluentAssertions;
using OrangeJuice.Server.Data.Models;
using Xunit;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	public class AwsArgumentBuilderTest
	{
		#region Test methods
		[Fact]
		public void BuildArgs_Should_Add_Default_Arguments()
		{
			// Arrange
			const string accessKey = "key";
			const string associateTag = "tag";

			DateTime now = DateTime.UtcNow;
			string timestamp = Convert.ToString(now);
			var dateTimeProvider = CreateDateTimeProvider(now);

			var argumentBuilder = CreateArgumentBuilder(new AwsOptions { AccessKey = accessKey, AssociateTag = associateTag }, dateTimeProvider.Object);

			// Act
			var args = argumentBuilder.BuildArgs(new ProductDescriptorSearchCriteria());

			// Assert
			args.Should().Contain("AWSAccessKeyId", accessKey)
				.And.Contain("AssociateTag", associateTag)
				.And.Contain("Service", "AWSECommerceService")
				.And.Contain("Condition", "All")
				.And.Contain("Timestamp", timestamp);
		}

		[Fact]
		public void BuildArgs_Should_Call_DateTimeProvider_GetNow()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new ProductDescriptorSearchCriteria());

			// Assert
			dateTimeProviderMock.VerifyAll();
		}

		[Fact]
		public void BuildArgs_Should_Call_DateTimeProvider_Format()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new ProductDescriptorSearchCriteria());

			// Assert
			dateTimeProviderMock.VerifyAll();
		}

		[Fact]
		public void BuildArgs_Should_Pass_Result_Of_DateTimeProvider_GetNow_To_DateTimeProvider_Format()
		{
			// Arrange
			DateTime now = DateTime.UtcNow;
			var dateTimeProviderMock = CreateDateTimeProvider(now);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new ProductDescriptorSearchCriteria());

			// Assert
			dateTimeProviderMock.VerifyAll();
		}
		#endregion

		#region Helper methods
		private static AwsArgumentBuilder CreateArgumentBuilder(AwsOptions awsOptions = null, IDateTimeProvider dateTimeProvider = null)
		{
			return new AwsArgumentBuilder(
				awsOptions ?? new AwsOptions(),
				dateTimeProvider ?? CreateDateTimeProvider(DateTime.UtcNow).Object);
		}

		private static Mock<IDateTimeProvider> CreateDateTimeProvider(DateTime now)
		{
			var dateTimeProviderMock = new Mock<IDateTimeProvider>();
			dateTimeProviderMock.Setup(p => p.GetNow()).Returns(now);
			dateTimeProviderMock.Setup(p => p.Format(It.IsAny<DateTime>())).Returns(Convert.ToString(now));
			return dateTimeProviderMock;
		}
		#endregion
	}
}