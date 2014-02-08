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
	public class ErrorDetailPolicyFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Create_Should_Call_ErrorDetailPolicyProvider_GetPolicies()
		{
			// Arrange
			IErrorDetailPolicyProvider provider = CreateErrorDetailPolicyProvider();
			ErrorDetailPolicyFactory factory = CreateFactory(provider);

			// Act
			factory.Create();

			// Assert
			Mock.Get(provider).Verify(p => p.GetPolicies(), Times.Once);
		}

		[TestMethod]
		public void Create_Should_Call_EnvironmentProvider_GetCurrentEnvironment()
		{
			// Arrange
			IEnvironmentProvider provider = CreateEnvironmentProvider();
			ErrorDetailPolicyFactory factory = CreateFactory(environmentProvider: provider);

			// Act
			factory.Create();

			// Assert
			Mock.Get(provider).Verify(p => p.GetCurrentEnvironment(), Times.Once);
		}

		[TestMethod]
		public void Create_Should_Return_Policy_Based_On_Environment()
		{
			// Arrange
			const string environment = Environment.Testing;
			const IncludeErrorDetailPolicy expected = IncludeErrorDetailPolicy.Never;

			IErrorDetailPolicyProvider errorDetailPolicyProvider = CreateErrorDetailPolicyProvider(environment, expected);
			IEnvironmentProvider environmentProvider = CreateEnvironmentProvider(environment);
			ErrorDetailPolicyFactory factory = CreateFactory(errorDetailPolicyProvider, environmentProvider);

			// Act
			IncludeErrorDetailPolicy actual = factory.Create();

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static ErrorDetailPolicyFactory CreateFactory(IErrorDetailPolicyProvider errorDetailPolicyProvider = null, IEnvironmentProvider environmentProvider = null)
		{
			return new ErrorDetailPolicyFactory(
				errorDetailPolicyProvider ?? CreateErrorDetailPolicyProvider(),
				environmentProvider ?? CreateEnvironmentProvider());
		}

		private static IErrorDetailPolicyProvider CreateErrorDetailPolicyProvider(string environment = Environment.Testing, IncludeErrorDetailPolicy policy = IncludeErrorDetailPolicy.Default)
		{
			var errorDetailPolicyProvider = new Mock<IErrorDetailPolicyProvider>();
			errorDetailPolicyProvider.Setup(p => p.GetPolicies()).Returns(
				new Dictionary<string, IncludeErrorDetailPolicy> { { environment, policy } });
			return errorDetailPolicyProvider.Object;
		}

		private static IEnvironmentProvider CreateEnvironmentProvider(string environment = Environment.Testing)
		{
			var environmentProviderMock = new Mock<IEnvironmentProvider>();
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);
			return environmentProviderMock.Object;
		}
		#endregion
	}
}