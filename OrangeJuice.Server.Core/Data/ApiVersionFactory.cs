using System.Reflection;

namespace OrangeJuice.Server.Data
{
	public sealed class ApiVersionFactory : IFactory<ApiVersion>
	{
		#region Fields
		private readonly IAssemblyProvider _assemblyProvider;
		#endregion

		#region Ctor
		public ApiVersionFactory(IAssemblyProvider assemblyProvider)
		{
			_assemblyProvider = assemblyProvider;
		}
		#endregion

		#region IApiVersionFactory members
		public ApiVersion Create()
		{
			return new ApiVersion
			{
				Version = GetVersion()
			};
		}
		#endregion

		#region Methods
		private string GetVersion()
		{
			return _assemblyProvider.GetExecutingAssembly()
			                        .GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
		}
		#endregion
	}
}