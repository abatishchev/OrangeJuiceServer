using System;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Controllers;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class UserController : ApiController
	{
		#region Fields
		private readonly IUserRepository _userRepository;
		private readonly IUrlProvider _urlProvider;

		#endregion

		#region Ctor
		public UserController(IUserRepository userRepository, IUrlProvider urlProvider)
		{
			_userRepository = userRepository;
			_urlProvider = urlProvider;
		}

		#endregion

		#region HTTP methods
		[Authorize]
		[Route("api/user")]
		public async Task<IHttpActionResult> GetUser([FromUri]UserSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			User user = await _userRepository.Search(searchCriteria.UserId);
			if (user == null)
				return this.NoContent();

			return Ok(user);
		}

		[Route("api/user")]
		public async Task<IHttpActionResult> PostUserUpdate(UserModel userModel)
		{
			if (userModel == null)
				throw new ArgumentNullException();

			await _userRepository.Update(userModel.Email, userModel.Name);

			return Ok();
		}

		[Route("api/user")]
		public async Task<IHttpActionResult> PutUserRegister(UserModel userModel)
		{
			if (userModel == null)
				throw new ArgumentNullException();

			User user = await _userRepository.Register(userModel.Email, userModel.Name);

			var url = _urlProvider.UriFor<UserController>(c => c.GetUser(new UserSearchCriteria { UserId = user.UserId }));
			return Created(url, user);
		}
		#endregion
	}
}