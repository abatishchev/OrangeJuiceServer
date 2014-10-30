using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Factory;

namespace OrangeJuice.Server.Api.Handlers
{
	public class AccessTokenAuthorizationHandler : DelegatingHandler
	{
		#region Fields
		private readonly IValidator<HttpRequestMessage, string> _validator;
		private readonly IFactory<IPrincipal, string> _principalFactory;
		#endregion

		#region Ctor
		public AccessTokenAuthorizationHandler(IValidator<HttpRequestMessage, string> validator, IFactory<IPrincipal, string> principalFactory)
		{
			_validator = validator;
			_principalFactory = principalFactory;
		}
		#endregion

		#region DelegatingHandler members
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			try
			{
				if (!_validator.IsValid(request))
					return base.SendAsync(request, cancellationToken);

				IPrincipal principal = _principalFactory.Create(_validator.ValidationResult);

				Thread.CurrentPrincipal = principal;
				if (HttpContext.Current != null)
					HttpContext.Current.User = principal;

				return base.SendAsync(request, cancellationToken);
			}
			catch (System.IdentityModel.Tokens.SecurityTokenExpiredException ex)
			{
				return Task.FromResult(request.CreateErrorResponse(HttpStatusCode.Forbidden, ex));
			}
			catch (Exception ex)
			{
				return Task.FromResult(request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex));
			}
		}
		#endregion
	}
}