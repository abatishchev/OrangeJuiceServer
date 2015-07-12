using System;

using Factory;
using FluentAssertions;

using OrangeJuice.Server.Configuration;

using Xunit;

namespace OrangeJuice.Server.Test.Configuration
{
	public class AwsOptionsFactoryTest
	{
		#region Tests
		[Theory]
		[InlineData(typeof(AwsOptionsFactory))]
		[InlineData(typeof(FSharp.Configuration.AwsOptionsFactory))]
		public void Create_Should_Return_AwsOptions_Having_All_Properties(Type type)
		{
			// Arrange
			var factory = CreateFactory(type);

			// Act
			AwsOptions options = factory.Create();

			// Assert
			options.Should().NotBeNull();
			options.AccessKey.Should().NotBeNullOrEmpty();
			options.AssociateTag.Should().NotBeNullOrEmpty();
			options.SecretKey.Should().NotBeNullOrEmpty();
			options.RequestLimit.Should().NotBe(TimeSpan.Zero);
		}
		#endregion

		#region Helpers methods
		private static IFactory<AwsOptions> CreateFactory(Type type)
		{
			return (IFactory<AwsOptions>)Activator.CreateInstance(type);
		}
		#endregion
	}
}