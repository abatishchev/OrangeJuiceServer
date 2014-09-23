﻿using System.Configuration;

namespace OrangeJuice.Server.Configuration
{
	public sealed class AppSettingsConfigurationProvider : IConfigurationProvider
	{
		public string GetValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}