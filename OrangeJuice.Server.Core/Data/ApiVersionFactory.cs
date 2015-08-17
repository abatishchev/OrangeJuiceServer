using System.Reflection;

using Ab.Configuration;
using Ab.Factory;
using Ab.Reflection;

using OrangeJuice.Server.Data.Models;

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
			_assemblyProvider = assemblyProvider;
			_environmentProvider = environmentProvider;
		}
		#endregion

		#region IApiVersionFactory members
		public ApiVersion Create()
		{
			return new ApiVersion
			{
				Version = GetVersion(),
				Environment = GetEnvironment()
			};
		}
		#endregion

		#region Methods
		private string GetVersion()
		{
			return _assemblyProvider.GetExecutingAssembly()
									.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
		}

		private string GetEnvironment()
		{
			return _environmentProvider.GetCurrentEnvironment();
		}
		#endregion
	}
}