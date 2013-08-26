using System.Reflection;

namespace OrangeJuice.Server.Api.Models
{
	public class ApiInfo
	{
		private ApiInfo()
		{
		}

		public string Version { get; set; }

		public static ApiInfo Create()
		{
			return new ApiInfo
			{
				Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version
			};
		}
	}
}