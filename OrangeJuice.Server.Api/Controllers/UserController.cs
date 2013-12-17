using System;
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
		public async Task<IHttpActionResult> GetUserInformation([FromUri]UserSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				return BadRequest("SearchCriteria is null");
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
		/// <url>PUT /api/user/</url>
		public async Task<IHttpActionResult> PutUserRegistration([FromBody]UserRegistration userRegistration)
		{
			if (userRegistration == null)
				return BadRequest("UserRegistration is null");
			if (!ModelState.IsValid)
				return BadRequest("Model is not valid");

			IUser user = await _userRepository.Register(userRegistration.Email);
			if (user == null)
				return InternalServerError(new InvalidOperationException("User is null"));

			return Ok(user.UserGuid);
		}
		#endregion
	}
}