using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class UserController : ApiController
	{
		#region Fields
		private readonly IUserRepository _userRepository;
		private readonly UrlHelper _urlHelper;

		#endregion

		#region Ctor
		public UserController(IUserRepository userRepository, UrlHelper urlHelper)
		{
			_userRepository = userRepository;
			_urlHelper = urlHelper;
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

			// TODO: make type-safe
			var url = _urlHelper.Link("GetUser", new { userid = user.UserId });

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