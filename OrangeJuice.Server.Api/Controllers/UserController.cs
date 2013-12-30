﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model;

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
			_userRepository = userRepository;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retrieves a user
		/// </summary>
		/// <param name="searchCriteria">User search criteria</param>
		/// <returns>User entity</returns>
		/// <url>GET /api/user</url>
		[ResponseType(typeof(User))]
		public async Task<IHttpActionResult> GetUserInformation([FromUri]UserSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest("Model is not valid");

			IUser user = await _userRepository.SearchByGuid(searchCriteria.UserGuid.GetValueOrDefault());
			if (user == null)
				return NotFound();

			return Ok(user);
		}

		/// <summary>
		/// Registers a user
		/// </summary>
		/// <param name="userRegistration">User userRegistration information</param>
		/// <returns>Guid representing the user</returns>
		/// <url>PUT /api/user</url>
		[ResponseType(typeof(Guid))]
		public async Task<IHttpActionResult> PutUserRegistration([FromBody]UserRegistration userRegistration)
		{
			if (!ModelState.IsValid)
				return BadRequest("Model is not valid");

			IUser user = await _userRepository.Register(userRegistration.Email);
			if (user == null)
				return InternalServerError();

			return Ok(user.UserGuid);
		}
		#endregion
	}
}