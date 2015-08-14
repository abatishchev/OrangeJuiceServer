using System;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using OrangeJuice.Server.Configuration;
using Xunit;

namespace OrangeJuice.Server.Test.Configuration
{
	public class DynamicAwsOptionsConverterTest
	{
		[Theory]
		[InlineData(typeof(DynamicAwsOptionsConverter))]
		[InlineData(typeof(FSharp.Configuration.DynamicAwsOptionsConverter))]
		public void Convert_And_ConverterBack_Should_Convert_DynamicTableEntity_To_AwsOptions_And_Back(Type type)
		{
			// Arrange
			var expected = new AwsOptions
			{
				AssociateTag = "associateTag",
				AccessKey = "accessKey",
				SecretKey = "secretKey",
				RequestLimit = TimeSpan.FromMilliseconds(500)
			};

			var converter = CreateConverter(type);

			// Act
			DynamicTableEntity entity = converter.ConvertBack(expected);
			AwsOptions actual = converter.Convert(entity);

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}

		private static IConverter<DynamicTableEntity, AwsOptions> CreateConverter(Type type)
		{
			return (IConverter<DynamicTableEntity, AwsOptions>)Activator.CreateInstance(type);
		}
	}
}