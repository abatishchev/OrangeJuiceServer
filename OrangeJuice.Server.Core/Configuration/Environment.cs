using System;

namespace OrangeJuice.Server.Configuration
{
	public static class Environment
	{
		#region Constants
		public const string Local = "Local";
		public const string Test = "Test";
		public const string Development = "Development";
		public const string Staging = "Staging";
		public const string AzureProduction = "AzureProduction";
		#endregion

		#region Properties
		public static string Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}