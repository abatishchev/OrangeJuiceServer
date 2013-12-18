using System;
using System.Reflection;

using OrangeJuice.Server.Configuration;

using Environment = OrangeJuice.Server.Configuration.Environment;

namespace OrangeJuice.Server.Data
{
	public sealed class ApiVersionFactory : IFactory<ApiVersion>
	{
		#region Fields
		private readonly IAssemblyProvider _assemblyProvider;
		private readonly IEnvironmentProvider _environmentProvider;
		#endregion

		#region Ctor
		public ApiVersionFactory(IAssemblyProvider assemblyProvider, IEnvironmentProvider environmentProvider)
		{
			if (assemblyProvider == null)
				throw new ArgumentNullException("assemblyProvider");
			if (environmentProvider == null)
				throw new ArgumentNullException("environmentProvider");

			_environmentProvider = environmentProvider;
			_assemblyProvider = assemblyProvider;
		}
		#endregion

		#region IApiVersionFactory Members
		public ApiVersion Create()
		{
			return new ApiVersion
			{
				Version = GetVersion(),
				Key = GetKey()
			};
		}
		#endregion

		#region Methods
		private string GetVersion()
		{
			return _assemblyProvider.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
		}

		private Guid? GetKey()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case Environment.Local:
				case Environment.Testing:
				case Environment.Development:
					return AppKey.Version0;
				case Environment.Staging:
				case Environment.Production:
					return null;
				default:
					throw new NotSupportedException(String.Format("Environment '{0}' is not supported", environment));
			}
		}
		#endregion
	}
}