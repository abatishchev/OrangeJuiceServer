using System;

namespace OrangeJuice.Server.Services
{
	public interface IUrlBuilder
	{
		Uri BuildUrl(Data.ProductDescriptorSearchCriteria searchCriteria);
	}
}