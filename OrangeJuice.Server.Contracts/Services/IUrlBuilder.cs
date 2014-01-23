using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public interface IUrlBuilder
	{
		Uri BuildUrl(IDictionary<string, string> args);
	}
}