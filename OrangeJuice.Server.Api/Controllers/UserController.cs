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
		/// <param name="information">??</param>
		/// <returns>User entity</returns>
		/// <url>GET /api/user/</url>
		public HttpResponseMessage GetUserInformation([FromUri]UserInformation information)
		{
			if (information == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("information"));

			if (!ModelValidator.Current.IsValid(this.ModelState))
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is not valid");

			IUser user = _userRepository.Find(information.UserKey.GetValueOrDefault());
			if (user == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return Request.CreateResponse(HttpStatusCode.OK, user);
		}

		/// <summary>
		/// Registers a user
		/// </summary>
		/// <param name="registration">User registration information</param>
		/// <returns>Guid representing the user</returns>
		/// <url>PUT /api/user/</url>
		public HttpResponseMessage PutUserRegistration([FromBody]UserRegistration registration)
		{
			if (registration == null)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new ArgumentNullException("registration"));

			if (!ModelValidator.Current.IsValid(this.ModelState))
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model is not valid");

			IUser user = _userRepository.Register(registration.Email);
			if (user == null)
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User is null");

			return Request.CreateResponse(HttpStatusCode.OK, user.UserGuid);
		}
	}
}