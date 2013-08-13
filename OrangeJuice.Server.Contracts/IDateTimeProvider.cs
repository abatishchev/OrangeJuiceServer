using System;

namespace OrangeJuice.Server
{
	public interface IDateTimeProvider
	{
		string FormatToUniversal(DateTime dateTime);
	}
}