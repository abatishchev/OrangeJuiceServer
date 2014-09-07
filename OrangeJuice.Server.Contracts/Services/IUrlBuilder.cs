using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public interface IUrlBuilder
	{
		Uri BuildUrl(Data.ProductDescriptorSearchCriteria searchCriteria);
	}
}