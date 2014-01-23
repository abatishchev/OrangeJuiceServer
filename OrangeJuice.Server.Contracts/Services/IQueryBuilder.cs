using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public interface IQueryBuilder
	{
		Uri BuildUrl(IDictionary<string, string> args);
	}
}