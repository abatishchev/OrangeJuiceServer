using System;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IUrlBuilder
	{
		Uri BuildUrl(AwsProductSearchCriteria searchCriteria);
	}
}