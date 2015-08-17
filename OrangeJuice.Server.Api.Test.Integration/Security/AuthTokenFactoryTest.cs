using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ab.Factory;
using FluentAssertions;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;
using SimpleInjector;
using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Security
{
	public class AuthTokenFactoryTest
	{
		[Theory]
		[MemberData("GetTypes")]
		public async Task Create_Should_Return_AuthToken_Having_All_Properties(Type googleAuthTokenFactoryType, Type authTokenFactoryType)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			var jwtFactory = container.GetInstance<IFactory<Jwt>>();
			var googleTokenFactory = (IFactory<Task<AuthToken>, string>)container.GetInstance(googleAuthTokenFactoryType);
			var bearerTokenFactory = (IFactory<Task<AuthToken>, AuthToken>)container.GetInstance(authTokenFactoryType);

			var jwt = jwtFactory.Create();
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt.Value);

			// Act
			AuthToken authToken = await bearerTokenFactory.Create(authorizationToken);

			// Assert
			authToken.Should().NotBeNull();
			authToken.AccessToken.Should().NotBeNullOrEmpty();
			authToken.IdToken.Should().NotBeNullOrEmpty();
			authToken.TokenType.Should().Be("bearer");
		}

		public static IEnumerable<object[]> GetTypes
		{
			get
			{
				yield return new[] { typeof(GoogleAuthTokenFactory), typeof(AuthTokenFactory) };
				//yield return new[] { typeof(FSharp.Security.GoogleAuthTokenFactory), typeof(FSharp.Security.AuthTokenFactory) };
			}
		}
	}
}