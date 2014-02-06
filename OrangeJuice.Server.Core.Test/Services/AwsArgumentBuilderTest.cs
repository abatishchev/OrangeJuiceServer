using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsArgumentBuilderTest
	{
		#region Test methods
		[TestMethod]
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
			var args = argumentBuilder.BuildArgs(new StringDictionary());

			// Assert
			args.Should().Contain("AWSAccessKeyId", accessKey)
				.And.Contain("AssociateTag", associateTag)
				.And.Contain("Service", "AWSECommerceService")
				.And.Contain("Condition", "All")
				.And.Contain("Timestamp", timestamp);
		}

		[TestMethod]
		public void BuildArgs_Should_Add_Arguments()
		{
			// Arange
			const string key = "key";
			const string value = "value";

			var queryBuilder = CreateArgumentBuilder();

			// Act
			var args = queryBuilder.BuildArgs(new StringDictionary { { key, value } });

			// Assert
			args.Should().Contain(key, value);
		}

		[TestMethod]
		public void BuildArgs_Should_Call_DateTimeProvider_GetNow()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var args = new StringDictionary();

			// Act
			queryBuilder.BuildArgs(args);

			// Assert
			dateTimeProviderMock.Verify(p => p.GetNow(), Times.Once);
		}

		[TestMethod]
		public void BuildArgs_Should_Call_DateTimeProvider_Format()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new StringDictionary());

			// Assert
			dateTimeProviderMock.Verify(p => p.Format(It.IsAny<DateTime>()), Times.Once);
		}

		[TestMethod]
		public void BuildArgs_Should_Pass_Result_Of_DateTimeProvider_GetNow_To_DateTimeProvider_Format()
		{
			// Arrange
			DateTime now = DateTime.UtcNow;
			var dateTimeProviderMock = CreateDateTimeProvider(now);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);

			// Act
			queryBuilder.BuildArgs(new StringDictionary());

			// Assert
			dateTimeProviderMock.Verify(p => p.Format(now), Times.Once);
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