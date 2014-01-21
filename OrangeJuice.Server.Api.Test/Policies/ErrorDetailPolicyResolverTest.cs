using System.Collections.Generic;
using System.Web.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Policies;
using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Test.Policies
{
	[TestClass]
	public class ErrorDetailPolicyResolverTest
	{
		#region Test methods
		[TestMethod]
		public void Resolve_Should_Call_ErrorDetailPolicyProvider_GetPolicies()
		{
			// Arrange
			var errorDetailPolicyProvider = CreateErrorDetailPolicyProvider();
			var resolver = CreateResolver(errorDetailPolicyProvider.Object);

			// Act
			resolver.Resolve();

			// Assert
			errorDetailPolicyProvider.Verify(p => p.GetPolicies(), Times.Once);
		}

		[TestMethod]
		public void Resolve_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			var environmentProvider = CreateEnvironmentProvider();
			var resolver = CreateResolver(environmentProvider: environmentProvider.Object);

			// Act
			resolver.Resolve();

			// Assert
			environmentProvider.Verify(p => p.GetCurrentEnvironment(), Times.Once);
		}

		[TestMethod]
		public void Resolve_Should_Return_Policy_Based_On_Environment()
		{
			// Arrange
			const IncludeErrorDetailPolicy expected = IncludeErrorDetailPolicy.Default;

			var errorDetailPolicyProvider = CreateErrorDetailPolicyProvider();
			var environmentProvider = CreateEnvironmentProvider();
			var resolver = CreateResolver(errorDetailPolicyProvider.Object, environmentProvider.Object);

			// Act
			IncludeErrorDetailPolicy actual = resolver.Resolve();

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static ErrorDetailPolicyResolver CreateResolver(IErrorDetailPolicyProvider errorDetailPolicyProvider = null, IEnvironmentProvider environmentProvider = null)
		{
			return new ErrorDetailPolicyResolver(
				errorDetailPolicyProvider ?? CreateErrorDetailPolicyProvider().Object,
				environmentProvider ?? CreateEnvironmentProvider().Object);
		}

		private static Mock<IErrorDetailPolicyProvider> CreateErrorDetailPolicyProvider(string environment = "", IncludeErrorDetailPolicy policy = IncludeErrorDetailPolicy.Default)
		{
			var errorDetailPolicyProvider = new Mock<IErrorDetailPolicyProvider>();
			errorDetailPolicyProvider.Setup(p => p.GetPolicies()).Returns(
				new Dictionary<string, IncludeErrorDetailPolicy> { { environment, policy } });
			return errorDetailPolicyProvider;
		}

		private static Mock<IEnvironmentProvider> CreateEnvironmentProvider(string environment = "")
		{
			var environmentProviderMock = new Mock<IEnvironmentProvider>();
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return environmentProviderMock;
		}
		#endregion
	}
}