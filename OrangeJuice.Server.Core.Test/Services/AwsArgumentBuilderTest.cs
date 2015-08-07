using System;

using FluentAssertions;
using OrangeJuice.Server.Data.Models;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Services;

using Xunit;

namespace OrangeJuice.Server.Test.Services
{
	public class AwsArgumentBuilderTest
	{
		#region Test methods
		[Theory]
		[InlineData(typeof(AwsArgumentBuilder))]
		[InlineData(typeof(FSharp.Services.AwsArgumentBuilder))]
		public void BuildArgs_Should_Add_Default_Arguments(Type type)
		{
			// Arrange
			const string accessKey = "key";
			const string associateTag = "tag";

			DateTime now = DateTime.UtcNow;
			string timestamp = Convert.ToString(now);
			var dateTimeProvider = CreateDateTimeProvider(now);

			var argumentBuilder = CreateArgumentBuilder(type, new AwsOptions { AccessKey = accessKey, AssociateTag = associateTag }, dateTimeProvider.Object);

			// Act
			var args = argumentBuilder.BuildArgs(new AwsProductSearchCriteria());

			// Assert
			args.Should().Contain("AWSAccessKeyId", accessKey)
				.And.Contain("AssociateTag", associateTag)
				.And.Contain("Service", "AWSECommerceService")
				.And.Contain("Condition", "All")
				.And.Contain("Timestamp", timestamp);
		}

		[Theory]
		[InlineData(typeof(AwsArgumentBuilder))]
		[InlineData(typeof(FSharp.Services.AwsArgumentBuilder))]
		public void BuildArgs_Should_Call_DateTimeProvider_GetNow(Type type)
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(type, dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new AwsProductSearchCriteria());

			// Assert
			dateTimeProviderMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(AwsArgumentBuilder))]
		[InlineData(typeof(FSharp.Services.AwsArgumentBuilder))]
		public void BuildArgs_Should_Call_DateTimeProvider_Format(Type type)
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(type, dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new AwsProductSearchCriteria());

			// Assert
			dateTimeProviderMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(AwsArgumentBuilder))]
		[InlineData(typeof(FSharp.Services.AwsArgumentBuilder))]
		public void BuildArgs_Should_Pass_Result_Of_DateTimeProvider_GetNow_To_DateTimeProvider_Format(Type type)
		{
			// Arrange
			DateTime now = DateTime.UtcNow;
			var dateTimeProviderMock = CreateDateTimeProvider(now);

			var queryBuilder = CreateArgumentBuilder(type, dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new AwsProductSearchCriteria());

			// Assert
			dateTimeProviderMock.VerifyAll();
		}
		#endregion

		#region Helper methods
		private static IArgumentBuilder CreateArgumentBuilder(Type type, AwsOptions awsOptions = null, IDateTimeProvider dateTimeProvider = null)
		{
			return (IArgumentBuilder)Activator.CreateInstance(type,
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