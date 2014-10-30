using System.IdentityModel.Tokens;
using System.Security.Principal;

using Factory;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Security
{
	public sealed class PrincipalFactoryFactory : IFactory<IFactory<IPrincipal, string>>
	{
		#region Fields
		private readonly IEnvironmentProvider _environmentProvider;
		private readonly ISecurityTokenValidator _tokenValidator;
		private readonly IFactory<TokenValidationParameters> _parametersFactory;
		#endregion

		#region Ctor
		public PrincipalFactoryFactory(IEnvironmentProvider environmentProvider, ISecurityTokenValidator tokenValidator, IFactory<TokenValidationParameters> parametersFactory)
		{
			_environmentProvider = environmentProvider;
			_tokenValidator = tokenValidator;
			_parametersFactory = parametersFactory;
		}
		#endregion

		#region IConverter members
		public IFactory<IPrincipal, string> Create()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case EnvironmentName.Local:
					return new GenericPrincipalFactory();
				default:
					return new SecurityTokenPrincipalFactory(_tokenValidator, _parametersFactory);
			}
		}
		#endregion
	}
}