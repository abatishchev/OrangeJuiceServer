using System;
using System.Collections.Generic;
using System.Configuration;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	public class TempConnectionString : IDisposable
	{
		#region Fields
		private readonly IDictionary<string, string> _originalValues = new Dictionary<string, string>();
		#endregion

		#region Constructors
		public TempConnectionString(string key, string value)
			: this(new KeyValuePair<string, string>(key, value))
		{
		}

		public TempConnectionString(params KeyValuePair<string, string>[] pairs)
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			foreach (var pair in pairs)
			{
				var originalSetting = config.AppSettings.Settings[pair.Key];
				if (originalSetting == null)
				{
					_originalValues.Add(pair.Key, null);
					config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(pair.Key, pair.Value));
				}
				else
				{
					_originalValues.Add(pair.Key, originalSetting.Value);
					config.ConnectionStrings.ConnectionStrings[pair.Key].ConnectionString = pair.Value;
				}
			}
			config.Save();
			ConfigurationManager.RefreshSection("connectionStrings");
		}
		#endregion

		#region Methods
		public void Dispose()
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			foreach (string key in _originalValues.Keys)
			{
				var originalValue = _originalValues[key];
				if (originalValue != null)
					config.ConnectionStrings.ConnectionStrings[key].ConnectionString = _originalValues[key];
				else
					config.ConnectionStrings.ConnectionStrings.Remove(key);

			}
			config.Save();
			ConfigurationManager.RefreshSection("connectionStrings");
		}
		#endregion
	}
}