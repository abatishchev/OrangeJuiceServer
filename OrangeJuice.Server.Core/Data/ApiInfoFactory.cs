using System;
using System.Reflection;

namespace OrangeJuice.Server.Data
{
	public class ApiInfoFactory : IApiInfoFactory
	{
		private readonly Lazy<ApiInfo> _instance = new Lazy<ApiInfo>(CreateInstance);

		private static ApiInfo CreateInstance()
		{
			// TODO: refactor with AssemblyProvider
			return new ApiInfo
			{
				Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version
			};
		}

		public ApiInfo Create()
		{
			return _instance.Value;
		}
	}
}