using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Policies;
using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Test.Policies
{
	[TestClass]
	public class ErrorDetailPolicyResolverTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Provider_Is_Null()
		{
			Assert.Inconclusive("TODO");

			// Arrange
			const IEnvironmentProvider environmentProvider = null;

			// Act
			var resolver = new ErrorDetailPolicyResolver(environmentProvider);

			// Assert
		}
	}
}