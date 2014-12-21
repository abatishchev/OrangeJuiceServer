using System;
using System.Threading.Tasks;

using Factory;
using FluentAssertions;

using OrangeJuice.Server.Data.Models;

using SimpleInjector;
using Xunit.Extensions;

namespace OrangeJuice.Server.Api.Test.Integration.Security
{
	public class AuthTokenFactoryTest
	{
		[Theory]
		[InlineData(typeof(Server.Security.AuthTokenFactory))]
		[InlineData(typeof(Server.FSharp.Security.AuthTokenFactory))]
		public async Task Create_Should_Return_AuthToken_Having_All_Properties(Type type)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			var jwtFactory = container.GetInstance<IFactory<string>>();
			var googleTokenFactory = container.GetInstance<IFactory<Task<AuthToken>, string>>();
			var bearerTokenFactory = CreateFactory(container, type);

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

		private static IFactory<Task<AuthToken>, AuthToken> CreateFactory(Container container, Type type)
		{
			return (IFactory<Task<AuthToken>, AuthToken>)container.GetInstance(type);
		}
	}
}