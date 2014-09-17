using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using Drum;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class UserController : ApiController
	{
		#region Fields
		private readonly IUserRepository _userRepository;
		private readonly UriMaker<UserController> _urlMaker;

		#endregion

		#region Ctor
		public UserController(IUserRepository userRepository, UriMaker<UserController> urlMaker)
		{
			_userRepository = userRepository;
			_urlMaker = urlMaker;
		}

		#endregion

		#region HTTP methods
		[Route("api/user", Name = "GetUser")]
		public async Task<IHttpActionResult> GetUser([FromUri]UserSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			IUser user = await _userRepository.Search(searchCriteria.UserId);
			if (user == null)
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(user);
		}

		[Route("api/user")]
		public async Task<IHttpActionResult> PutUser(UserModel userModel)
		{
			if (userModel == null)
				throw new ArgumentNullException();

			// TODO: handle duplication
			IUser user = await _userRepository.Register(userModel.Email, userModel.Name);

			var url = _urlMaker.UriFor(c => c.GetUser(new UserSearchCriteria { UserId = user.UserId }));

			return Created(url, user);
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