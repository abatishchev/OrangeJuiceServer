using System;
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
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Provider_Is_Null()
		{
			// Arrange
			const IEnvironmentProvider environmentProvider = null;

			// Act
			Action action = () => new ErrorDetailPolicyResolver(environmentProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("environmentProvider");
		}

		[TestMethod]
		public void Resolve_Should_Return_Policy_Based_On_Environment()
		{
			// Arrange
			const string environment = "Test";
			const IncludeErrorDetailPolicy expected = IncludeErrorDetailPolicy.Default;

			var environmentProviderMock = new Mock<IEnvironmentProvider>(MockBehavior.Strict);
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);

			var resolver = new ErrorDetailPolicyResolver(
				environmentProviderMock.Object,
				new Dictionary<string, IncludeErrorDetailPolicy>
				{
					{ environment, expected }
				});

			// Act
			IncludeErrorDetailPolicy actual = resolver.Resolve();

			// Assert
			actual.Should().Be(expected);
		}
		
		[TestMethod]
		public void Resolve_Should_Throw_Exception_When_Policies_Do_Not_Contain_Environment()
		{
			// Arrange
			const string environment = "Test";

			var environmentProviderMock = new Mock<IEnvironmentProvider>(MockBehavior.Strict);
			environmentProviderMock.Setup(p => p.GetCurrentEnvironment()).Returns(environment);

			var resolver = new ErrorDetailPolicyResolver(
				environmentProviderMock.Object,
				new Dictionary<string, IncludeErrorDetailPolicy>());

			// Act
			Action action = () => resolver.Resolve();

			// Assert
			action.ShouldThrow<InvalidOperationException>();
		}
	}
}