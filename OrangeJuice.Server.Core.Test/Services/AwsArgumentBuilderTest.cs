using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsArgumentBuilderTest
	{
		#region BuildArgs
		[TestMethod]
		public void BuildArgs_Should_Add_Default_Arguments()
		{
			// Arrange
			const string accessKey = "anyKey";
			const string associateTag = "anyTag";

			DateTime now = DateTime.UtcNow;
			string timestamp = Convert.ToString(now);
			var dateTimeProvider = CreateDateTimeProvider(now);

			var argumentBuilder = CreateArgumentBuilder(accessKey, associateTag, dateTimeProvider.Object);

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
		public void BuildArgs_Should_Return_Arguments_Containing_Input_Argument()
		{
			// Arange
			const string key = "anyKey";
			const string value = "anyValue";

			var queryBuilder = CreateArgumentBuilder();
			IStringDictionary args = new StringDictionary
			{
				 { key, value }
			};

			// Act
			args = queryBuilder.BuildArgs(args);

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
			dateTimeProviderMock.Verify(p => p.GetNow(), Times.Once());
		}

		[TestMethod]
		public void BuildArgs_Should_Call_DateTimeProvider_FormatToUniversal()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var args = new StringDictionary();

			// Act
			queryBuilder.BuildArgs(args);

			// Assert
			dateTimeProviderMock.Verify(p => p.FormatToUniversal(It.IsAny<DateTime>()), Times.Once());
		}

		[TestMethod]
		public void BuildArgs_Should_Pass_Result_Of_DateTimeProvider_GetNow_To_DateTimeProvider_FormatToUniversal()
		{
			// Arrange
			DateTime now = DateTime.UtcNow;
			var dateTimeProviderMock = CreateDateTimeProvider(now);

			var queryBuilder = CreateArgumentBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var args = new StringDictionary();

			// Act
			queryBuilder.BuildArgs(args);

			// Assert
			dateTimeProviderMock.Verify(p => p.FormatToUniversal(now), Times.Once());
		}
		#endregion

		#region Helper methods
		private static Mock<IDateTimeProvider> CreateDateTimeProvider(DateTime now)
		{
			var dateTimeProviderMock = new Mock<IDateTimeProvider>();
			dateTimeProviderMock.Setup(p => p.GetNow()).Returns(now);
			dateTimeProviderMock.Setup(p => p.FormatToUniversal(It.IsAny<DateTime>())).Returns(Convert.ToString(now));
			return dateTimeProviderMock;
		}

		private static AwsArgumentBuilder CreateArgumentBuilder(string accessKey = null, string associateTag = null, IDateTimeProvider dateTimeProvider = null)
		{
			return new AwsArgumentBuilder(
				accessKey ?? "anyKey",
				associateTag ?? "anyTag",
				dateTimeProvider ?? CreateDateTimeProvider(DateTime.UtcNow).Object);
		}
		#endregion
	}
}