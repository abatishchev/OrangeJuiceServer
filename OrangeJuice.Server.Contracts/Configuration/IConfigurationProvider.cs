using System;

namespace OrangeJuice.Server.Configuration
{
	public interface IConfigurationProvider
	{
		string GetValue(string key);
	}
}