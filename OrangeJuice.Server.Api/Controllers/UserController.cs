using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class UserController : ApiController
	{
		#region Fields
		private readonly IUserRepository _userRepository;
		#endregion

		#region Ctor
		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		#endregion

		#region HTTP methods
		/// <summary>
		/// Retrieves a user
		/// </summary>
		/// <returns>User entity</returns>
		/// <url>GET /api/user</url>
		public async Task<IHttpActionResult> GetUser([FromUri]UserSearchCriteria searchCriteria)
		{
			IUser user = await _userRepository.Search(searchCriteria.UserId);
			if (user == null)
				return NotFound();

			return Ok(user);
		}

		/// <summary>
		/// Registers a user
		/// </summary>
		/// <returns>Guid representing the user</returns>
		/// <url>PUT /api/user</url>
		public async Task<IHttpActionResult> PutUser(UserModel userModel)
		{
			IUser user = await _userRepository.Register(userModel.Email, userModel.Name);
			if (user == null)
				return InternalServerError();

			return Ok(user.UserId);
		}
		#endregion

		#region Methods
		protected override void Dispose(bool disposing)
		{
			_userRepository.Dispose();

			base.Dispose(disposing);
		}
		#endregion
	}
}