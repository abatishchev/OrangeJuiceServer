using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.FSharp.Services;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.FSharp.Test.Services
{
	[TestClass]
	public class JsonBlobNameResolverFSharpTest
	{
		[TestMethod]
		public void Resolve_Should_Append_Json()
		{
			// Arrange
			string blobName = "blobName";

			IBlobNameResolver resolver = new JsonBlobNameResolver();

			// Act
			blobName = resolver.Resolve(blobName);

			// Assert
			blobName.Should().EndWith(".json");
		}
	}
}