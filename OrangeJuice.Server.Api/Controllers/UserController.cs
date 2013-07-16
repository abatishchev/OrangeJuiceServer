using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Controllers
{
    public class UserController : ApiController
    {
        private readonly IModelValidator _modelValidator;

        public UserController(IModelValidator modelValidator)
        {
            _modelValidator = modelValidator;
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="appKey">Application version key</param>
        /// <param name="userRegistration">User registartion information</param>
        /// <returns>Guid representing the user</returns>
        /// <url>POST api/user</url>
        public HttpResponseMessage Post([FromBody]Guid appKey, UserRegistration userRegistration)
        {
            if (!_modelValidator.IsValid(this.ModelState))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Guid newGuid = Guid.NewGuid();
            return Request.CreateResponse(newGuid);
        }
    }
}