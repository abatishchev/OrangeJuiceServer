using System.Threading.Tasks;

using Factory;

using FluentAssertions;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Security
{
	public class AuthTokenFactoryTest
	{
		#region Tests
		[Fact]
		public async Task Create_Should_Return_AuthToken_Having_All_Properties()
		{
			using (IUnityContainer container = ContainerConfig.CreateWebApiContainer())
			{
				// Arrange
                var jwtFactory = container.Resolve<IFactory<string>>(typeof(JwtFactory).Name);
				var googleTokenFactory = container.Resolve<IFactory<Task<AuthToken>, string>>(typeof(GoogleAuthTokenFactory).Name);
                var bearerTokenFactory = container.Resolve<IFactory<Task<AuthToken>, AuthToken>>(typeof(AuthTokenFactory).Name);

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
		}
		#endregion
	}
}