using System;
using System.Reflection;
using System.Threading.Tasks;

using OrangeJuice.Server.Configuration;

using Environment = OrangeJuice.Server.Configuration.Environment;

namespace OrangeJuice.Server.Data
{
	public sealed class ApiVersionFactory : IApiVersionFactory
	{
		#region Fields
		private readonly IAssemblyProvider _assemblyProvider;
		private readonly IEnvironmentProvider _environmentProvider;

		private Task<ApiVersion> _instance;
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
		public async Task<ApiVersion> Create()
		{
			return await (_instance ?? (_instance = Task<ApiVersion>.Factory.StartNew(CreateInstance)));
		}
		#endregion

		#region Methods
		private ApiVersion CreateInstance()
		{
			return new ApiVersion
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
				case Environment.Test:
				case Environment.Development:
					return AppKey.Version0;
				default:
					return null;
			}
		}
		#endregion
	}
}