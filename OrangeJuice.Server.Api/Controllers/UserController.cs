using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class UserController : ApiController
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			if (userRepository == null)
				throw new ArgumentNullException("userRepository");

			_userRepository = userRepository;
		}

		/// <summary>
		/// Retrieves a user
		/// </summary>
		/// <param name="userGuid">Guid representing a user</param>
		/// <returns>User entity</returns>
		/// <url>GET /api/user/</url>
		public IUser Get(Guid userGuid)
		{
			if (userGuid == Guid.Empty)
				throw new ArgumentNullException("userGuid");

			IUser user = _userRepository.Find(userGuid);
			if (user == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return user;
		}

		/// <summary>
		/// Registers a user
		/// </summary>
		/// <param name="userRegistration">User registartion information</param>
		/// <returns>Guid representing the user</returns>
		/// <url>POST /api/user/</url>
		public HttpResponseMessage Put([FromBody]UserRegistration userRegistration)
		{
			if (userRegistration == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("userRegistration"));

			if (!ModelValidator.Current.IsValid(this.ModelState))
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is not valid");

			IUser user = _userRepository.Register(userRegistration.Email);
			if (user == null)
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User is null");

			return Request.CreateResponse(HttpStatusCode.OK, user.UserGuid);
		}
	}
}