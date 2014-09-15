using System;
using System.Net;
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
		[Route("api/user")]
		public async Task<IHttpActionResult> GetUser([FromUri]UserSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			IUser user = await _userRepository.Search(searchCriteria.UserId);
			if (user == null)
				return NotFound();

			return Ok(user);
		}

		[Route("api/user")]
		public async Task<IHttpActionResult> PutUser(UserModel userModel)
		{
			if (userModel == null)
				throw new ArgumentNullException();

			IUser user = await _userRepository.Register(userModel.Email, userModel.Name);

			// TODO: return Created
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