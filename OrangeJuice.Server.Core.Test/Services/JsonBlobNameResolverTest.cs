using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class JsonBlobNameResolverTest
	{
		[TestMethod]
		public void Resolve_Should_Appen_Json()
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