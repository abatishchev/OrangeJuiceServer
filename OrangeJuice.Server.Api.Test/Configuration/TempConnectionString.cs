using System;
using System.Configuration;
using System.Reflection;

namespace OrangeJuice.Server.Api.Test.Configuration
{
	internal class TempConnectionString : IDisposable
	{
		private readonly string _name;
		private readonly string _oldValue;

		public TempConnectionString(string name, string value)
		{
			_name = name;

			ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[name];
			if (connectionString != null)
			{
				_oldValue = connectionString.ConnectionString;
				connectionString.ConnectionString = value;
			}
			else
			{
				UpdateValue(name, value);
			}
		}

		private static void UpdateValue(string name, string value)
		{
			var connectionString = new ConnectionStringSettings(name, value, "System.Data.SqlClient");

			var readonlyField = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
			readonlyField.SetValue(ConfigurationManager.ConnectionStrings, false);

			var baseAddMethod = typeof(ConfigurationElementCollection).GetMethod("BaseAdd", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(ConfigurationElement) }, null);
			baseAddMethod.Invoke(ConfigurationManager.ConnectionStrings, new[] { connectionString });

			readonlyField.SetValue(ConfigurationManager.ConnectionStrings, true);
		}

		public void Dispose()
		{
			if (_oldValue != null)
				UpdateValue(_name, _oldValue);
			//else
			//UpdateValue(_name, "");
		}
	}
}