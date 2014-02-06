using System;

namespace OrangeJuice.Server
{
	public interface IDateTimeProvider
	{
		DateTime GetNow();

		string Format(DateTime dateTime);
	}
}