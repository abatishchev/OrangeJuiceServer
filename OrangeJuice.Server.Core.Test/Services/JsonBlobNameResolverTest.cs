using System;

using FluentAssertions;

using Xunit.Extensions;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	public class JsonBlobNameResolverTest
	{
		[Theory]
		[InlineData(typeof(JsonBlobNameResolver))]
		[InlineData(typeof(FSharp.Services.JsonBlobNameResolver))]
		public void Resolve_Should_Append_Json(Type type)
		{
			// Arrange
			string blobName = "blobName";

			IBlobNameResolver resolver = CreateResolver(type);

			// Act
			blobName = resolver.Resolve(blobName);

			// Assert
			blobName.Should().EndWith(".json");
		}

		private static IBlobNameResolver CreateResolver(Type type)
		{
			return (IBlobNameResolver)Activator.CreateInstance(type);
		}
	}
}