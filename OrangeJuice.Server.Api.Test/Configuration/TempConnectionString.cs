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

			SetWritable(ConfigurationManager.ConnectionStrings);

			ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[name];
			if (connectionString != null)
			{
				_oldValue = connectionString.ConnectionString;
				connectionString.ConnectionString = value;
			}
			else
			{
				SetValue(name, value);
			}
		}

		private static void SetWritable(ConnectionStringSettingsCollection connectionStrings)
		{
			FieldInfo readonlyField = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
			readonlyField.SetValue(connectionStrings, false);
		}

		private static void SetValue(string name, string value)
		{
			MethodInfo baseAddMethod = typeof(ConfigurationElementCollection).GetMethod("BaseAdd", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(ConfigurationElement) }, null);
			ConnectionStringSettings connectionString = new ConnectionStringSettings(name, value);
			baseAddMethod.Invoke(ConfigurationManager.ConnectionStrings, new object[] { connectionString });
		}

		private static void RemoveValue(string name)
		{
			ConfigurationManager.ConnectionStrings.Remove(name);
		}

		public void Dispose()
		{
			if (_oldValue != null)
				SetValue(_name, _oldValue);
			else
				RemoveValue(_name);
		}
	}
}