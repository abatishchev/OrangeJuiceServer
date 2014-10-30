using System.Threading.Tasks;

using Factory;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;

using SimpleInjector;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Security
{
	public class AuthTokenFactoryTest
	{
		#region Tests
		[Fact]
		public async Task Create_Should_Return_AuthToken_Having_All_Properties()
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();
			var jwtFactory = container.GetInstance<IFactory<string>>();
			var googleTokenFactory = container.GetInstance<IFactory<Task<AuthToken>, string>>();
			var bearerTokenFactory = container.GetInstance<IFactory<Task<AuthToken>, AuthToken>>();

			string jwt = jwtFactory.Create();
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt);

			// Act
			AuthToken authToken = await bearerTokenFactory.Create(authorizationToken);

			// Assert
			authToken.Should().NotBeNull();
			authToken.AccessToken.Should().NotBeNullOrEmpty();
			authToken.IdToken.Should().NotBeNullOrEmpty();
			authToken.TokenType.Should().NotBeNullOrEmpty();
		}
		#endregion
	}
}