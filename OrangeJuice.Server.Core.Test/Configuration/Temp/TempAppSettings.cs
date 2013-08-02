using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OrangeJuice.Server.Test.Configuration.Temp
{
	public class TempAppSettings : IDisposable
	{
		#region Fields
        private readonly IDictionary<string, string> _originalValues;
        #endregion

        #region Constructors
        public TempAppSettings(string key, string value)
            : this(new KeyValuePair<string, string>(key, value))
        {
        }

		public TempAppSettings(params KeyValuePair<string, string>[] values)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            this._originalValues = new Dictionary<string, string>();
            foreach (var value in values ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                var originalSetting = config.AppSettings.Settings[value.Key];
                if (originalSetting == null)
                {
                    this._originalValues.Add(value.Key, null);
                    config.AppSettings.Settings.Add(value.Key, value.Value);
                }
                else
                {
                    this._originalValues.Add(value.Key, originalSetting.Value);
                    config.AppSettings.Settings[value.Key].Value = value.Value;
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
            foreach (string key in this._originalValues.Keys)
            {
                var originalValue = this._originalValues[key];
                if (originalValue == null)
                    config.AppSettings.Settings.Remove(key);
                else
                    config.AppSettings.Settings[key].Value = this._originalValues[key];
            }
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
	}
}