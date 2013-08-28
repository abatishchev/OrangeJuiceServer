using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class UserController : ApiController
	{
		#region Fields
		private readonly IUserRepository _userRepository;
		#endregion

		#region Constructors
		public UserController(IUserRepository userRepository)
		{
			if (userRepository == null)
				throw new ArgumentNullException("userRepository");

			_userRepository = userRepository;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retrieves a user
		/// </summary>
		/// <param name="searchCriteria">User search criteria</param>
		/// <returns>User entity</returns>
		/// <url>GET /api/user/</url>
		public async Task<HttpResponseMessage> GetUserInformation([FromUri]UserSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("searchCriteria"));

			IUser user = await _userRepository.SearchByGuid(searchCriteria.UserGuid.GetValueOrDefault());
			if (user == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return Request.CreateResponse(HttpStatusCode.OK, user);
		}

		/// <summary>
		/// Registers a user
		/// </summary>
		/// <param name="userRegistration">User userRegistration information</param>
		/// <returns>Guid representing the user</returns>
		/// <url>PUT /api/user/</url>
		public async Task<HttpResponseMessage> PutUserRegistration([FromBody]UserRegistration userRegistration)
		{
			if (userRegistration == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("userRegistration"));

			IUser user = await _userRepository.Register(userRegistration.Email);
			if (user == null)
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User is null");

			return Request.CreateResponse(HttpStatusCode.OK, user.UserGuid);
		}
		#endregion
	}
}