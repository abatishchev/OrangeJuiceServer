using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using FluentAssertions;

using Xunit;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	public class UserControllerIntegrationTest
	{
		#region Tests
		[Fact]
		public async Task GetUser_Should_Return_Status_Ok_When_User_Exists()
		{
			// Arrange
			Guid userId = Data.Test.EntityFactory.Get<Data.User>().UserId;

			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("userId", userId.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/user";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetUser_Should_Return_Status_NoContent_When_User_Does_Not_Exist()
		{
			// Arrange
			Guid userId = Guid.NewGuid();

			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("userId", userId.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/user";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task PutUser_Should_Return_Status_Created()
		{
			// Arrange
			var user = new UserModel
			{
				Name = Guid.NewGuid().ToString(),
				Email = String.Format("{0}@example.com", Guid.NewGuid())
			};

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/user";

			// Act
			var response = await client.PutAsJsonAsync(url.Uri, user);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Created);
		}
		#endregion
	}
}