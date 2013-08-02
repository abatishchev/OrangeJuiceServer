using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	public class TempConnectionString : IDisposable
	{
		#region Fields
		private readonly string _name;
		private readonly string _oldValue;
		#endregion

		#region Constructors
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
		#endregion

		#region Methods
		// ReSharper disable once SuggestBaseTypeForParameter
		private static void SetWritable(ConnectionStringSettingsCollection connectionStrings)
		{
			FieldInfo fieldInfo = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
			Debug.Assert(fieldInfo != null, "fieldInfo is null");
			fieldInfo.SetValue(connectionStrings, false);
		}

		private static void SetValue(string name, string value)
		{
			MethodInfo methodInfo = typeof(ConfigurationElementCollection).GetMethod("BaseAdd", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(ConfigurationElement) }, null);
			Debug.Assert(methodInfo != null, "methodInfo is null");
			ConnectionStringSettings connectionString = new ConnectionStringSettings(name, value);
			methodInfo.Invoke(ConfigurationManager.ConnectionStrings, new object[] { connectionString });
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
		#endregion
	}
}