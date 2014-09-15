using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	[TestClass]
	public class UserControllerIntegrationTest
	{
		#region Tests
		[TestMethod]
		public async Task GetUser_Should_Return_Ok_When_User_Exists()
		{
			// Arrange
			var user = GetFirstUser();
			if (user == null)
				Assert.Inconclusive("Database contains no users");
			Guid? userId = GetFirstUser().UserId;

			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("userId", userId.Value.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/user";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[TestMethod]
		public async Task GetUser_Should_Return_NotFound_When_User_Does_Not_Exist()
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
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task PutUser_Should_Return_Ok_When_User_Was_Created()
		{
			// Arrange
			var user = NewUser();

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/user";

			// Act
			var response = await client.PutAsJsonAsync(url.Uri, user);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
		#endregion

		#region Helper methods
		private static UserModel NewUser()
		{
			return new UserModel
			{
				Name = Guid.NewGuid().ToString(),
				Email = String.Format("{0}@example.com", Guid.NewGuid())
			};
		}

		private static UserModel NewUser(User user)
		{
			return new UserModel
			{
				Name = user.Name,
				Email = user.Email
			};
		}

		private static User GetFirstUser()
		{
			try
			{
				using (var container = new ModelContainer())
				{
					return container.Users.FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				Assert.Inconclusive(ex.Message);
			}
			return null;
		}
		#endregion
	}
}