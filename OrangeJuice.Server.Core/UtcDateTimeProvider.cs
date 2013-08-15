using System;

namespace OrangeJuice.Server
{
	public sealed class UtcDateTimeProvider : IDateTimeProvider
	{
		internal const string UniversalDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

		public DateTime GetNow()
		{
			return DateTime.UtcNow;
		}

		/// <summary>
		/// Current time in IS0 8601 format as required by Amazon
		/// </summary>
		public string FormatToUniversal(DateTime dateTime)
		{
			return dateTime.ToString(UniversalDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}