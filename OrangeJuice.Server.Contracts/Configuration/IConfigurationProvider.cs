using System;

namespace OrangeJuice.Server.Configuration
{
	public interface IConfigurationProvider
	{
		string ReadValue(string key);
	}
}