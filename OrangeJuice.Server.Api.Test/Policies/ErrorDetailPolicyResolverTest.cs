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
			IErrorDetailPolicyProvider provider = CreateErrorDetailPolicyProvider();
			ErrorDetailPolicyResolver resolver = CreateResolver(provider);

			// Act
			resolver.Resolve();

			// Assert
			Mock.Get(provider).Verify(p => p.GetPolicies(), Times.Once);
		}

		[TestMethod]
		public void Resolve_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			IEnvironmentProvider provider = CreateEnvironmentProvider();
			ErrorDetailPolicyResolver resolver = CreateResolver(environmentProvider: provider);

			// Act
			resolver.Resolve();

			// Assert
			Mock.Get(provider).Verify(p => p.GetCurrentEnvironment(), Times.Once);
		}

		[TestMethod]
		public void Resolve_Should_Return_Policy_Based_On_Environment()
		{
			// Arrange
			const IncludeErrorDetailPolicy expected = IncludeErrorDetailPolicy.Default;

			IErrorDetailPolicyProvider errorDetailPolicyProvider = CreateErrorDetailPolicyProvider();
			IEnvironmentProvider environmentProvider = CreateEnvironmentProvider();
			ErrorDetailPolicyResolver resolver = CreateResolver(errorDetailPolicyProvider, environmentProvider);

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
				errorDetailPolicyProvider ?? CreateErrorDetailPolicyProvider(),
				environmentProvider ?? CreateEnvironmentProvider());
		}

		private static IErrorDetailPolicyProvider CreateErrorDetailPolicyProvider(string environment = "", IncludeErrorDetailPolicy policy = IncludeErrorDetailPolicy.Default)
		{
			var errorDetailPolicyProvider = new Mock<IErrorDetailPolicyProvider>();
			errorDetailPolicyProvider.Setup(p => p.GetPolicies()).Returns(
				new Dictionary<string, IncludeErrorDetailPolicy> { { environment, policy } });
			return errorDetailPolicyProvider.Object;
		}

		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = "")
		{
			var environmentProviderMock = new Mock<IEnvironmentProvider>();
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return environmentProviderMock.Object;
		}
		#endregion
	}
}