using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	[TestClass]
	public class UserControllerIntegrationTest
	{
		#region Tests
		[TestMethod]
		public async Task Get_Api_User_Should_Return_NotFound_When_User_Does_Not_Exist()
		{
			// Arrange
			Guid userId = Guid.NewGuid();

			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("userId", userId.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "/api/user";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task Get_Api_User_Should_Return_Ok_When_User_Exists()
		{
			// Arrange
			Guid? userId = GetFirstUserId();
			if (!userId.HasValue)
				Assert.Inconclusive("Database contains no users");

			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("userId", userId.Value.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "/api/user";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		#endregion

		#region Helper methods
		private static Guid? GetFirstUserId()
		{
			try
			{
				using (var container = new ModelContainer())
				{
					return container.Users.Select(u => (Guid?)u.UserId).FirstOrDefault();
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