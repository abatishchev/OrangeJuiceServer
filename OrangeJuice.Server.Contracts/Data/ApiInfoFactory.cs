﻿using System;
using System.Reflection;

namespace OrangeJuice.Server.Data
{
	public class ApiInfoFactory
	{
		private readonly Lazy<ApiInfo> _instance = new Lazy<ApiInfo>(CreateInstance);

		private static ApiInfo CreateInstance()
		{
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