using System;
using System.Collections.Generic;
using System.Configuration;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	public class TempAppSettings : IDisposable
	{
		#region Fields
		private readonly IDictionary<string, string> _originalValues = new Dictionary<string, string>();
		#endregion

		#region Constructors
		public TempAppSettings(string key, string value)
			: this(new KeyValuePair<string, string>(key, value))
		{
		}

		public TempAppSettings(params KeyValuePair<string, string>[] pairs)
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			foreach (var pair in pairs)
			{
				var originalSetting = config.AppSettings.Settings[pair.Key];
				if (originalSetting == null)
				{
					_originalValues.Add(pair.Key, null);
					config.AppSettings.Settings.Add(pair.Key, pair.Value);
				}
				else
				{
					_originalValues.Add(pair.Key, originalSetting.Value);
					config.AppSettings.Settings[pair.Key].Value = pair.Value;
				}
			}
			config.Save();
			ConfigurationManager.RefreshSection("appSettings");
		}
		#endregion

		#region Methods
		public void Dispose()
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			foreach (string key in _originalValues.Keys)
			{
				string originalValue = _originalValues[key];
				if (originalValue != null)
					config.AppSettings.Settings[key].Value = _originalValues[key];
				else
					config.AppSettings.Settings.Remove(key);

			}
			config.Save();
			ConfigurationManager.RefreshSection("appSettings");
		}
		#endregion
	}
}