using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException("userRepository");

            _userRepository = userRepository;
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userRegistration">User registartion information</param>
        /// <returns>Guid representing the user</returns>
        /// <url>POST api/user</url>
        public HttpResponseMessage Post([FromBody]UserRegistration userRegistration)
        {
            if (!ModelValidator.Current.IsValid(this.ModelState))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Guid newGuid = _userRepository.Register(userRegistration.Email);
            return Request.CreateResponse(HttpStatusCode.OK, newGuid);
        }
    }
}