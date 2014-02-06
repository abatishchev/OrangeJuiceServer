using System;
using System.Globalization;

namespace OrangeJuice.Server
{
	public sealed class UtcDateTimeProvider : IDateTimeProvider
	{
		#region Constants
		private const string UniversalDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
		#endregion

		#region IDateTimeProvider members
		public DateTime GetNow()
		{
			return DateTime.UtcNow;
		}

		/// <summary>
		/// Current time in IS0 8601 format
		/// </summary>
		public string Format(DateTime dateTime)
		{
			return dateTime.ToString(UniversalDateTimeFormat, CultureInfo.InvariantCulture);
		}
		#endregion
	}
}