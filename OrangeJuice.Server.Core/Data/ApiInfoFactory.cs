using System;
using System.Reflection;

using OrangeJuice.Server.Configuration;

using Environment = OrangeJuice.Server.Configuration.Environment;

namespace OrangeJuice.Server.Data
{
	public sealed class ApiInfoFactory : IApiInfoFactory
	{
		#region Fields
		private readonly IAssemblyProvider _assemblyProvider;
		private readonly IEnvironmentProvider _environmentProvider;

		private ApiInfo _instance;
		#endregion

		#region Ctor
		public ApiInfoFactory(IAssemblyProvider assemblyProvider, IEnvironmentProvider environmentProvider)
		{
			if (assemblyProvider == null)
				throw new ArgumentNullException("assemblyProvider");
			if (environmentProvider == null)
				throw new ArgumentNullException("environmentProvider");

			_environmentProvider = environmentProvider;
			_assemblyProvider = assemblyProvider;
		}
		#endregion

		#region IApiInfoFactory Members
		public ApiInfo Create()
		{
			return _instance ?? (_instance = CreateInstance());
		}
		#endregion

		#region Methods
		private ApiInfo CreateInstance()
		{
			return new ApiInfo
			{
				Version = GetVersion(),
				Key = GetKey()
			};
		}

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
				case Environment.Development:
					return AppKey.Version0;
				default:
					return null;
			}
		}
		#endregion
	}
}