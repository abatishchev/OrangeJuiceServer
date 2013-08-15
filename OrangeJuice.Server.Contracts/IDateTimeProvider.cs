using System;

namespace OrangeJuice.Server
{
	public interface IDateTimeProvider
	{
		DateTime GetNow();

		string FormatToUniversal(DateTime dateTime);
	}
}