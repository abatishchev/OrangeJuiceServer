using System;
using System.Collections.Generic;
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
		[Theory]
		[MemberData("GetTypes")]
		public async Task Create_Should_Return_AuthToken_Having_All_Properties(Type googleAuthTokenFactoryType, Type authTokenFactoryType)
		{
			// Arrange
			Container container = ContainerConfig.CreateWebApiContainer();

			var jwtFactory = container.GetInstance<IFactory<string>>();
			var googleTokenFactory = (IFactory<Task<AuthToken>, string>)container.GetInstance(googleAuthTokenFactoryType);
			var bearerTokenFactory = (IFactory<Task<AuthToken>, AuthToken>)container.GetInstance(authTokenFactoryType);

			string jwt = jwtFactory.Create();
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt);

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
				yield return new[] { typeof(Server.Security.GoogleAuthTokenFactory), typeof(Server.Security.AuthTokenFactory) };
				yield return new[] { typeof(Server.FSharp.Security.GoogleAuthTokenFactory), typeof(Server.FSharp.Security.AuthTokenFactory) };
			}
		}
	}
}