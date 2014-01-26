using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureFoodProvider : IFoodProvider
	{
		#region IFoodProvider members
		public Task<IEnumerable<FoodDescriptor>> Search(string title)
		{
			throw new NotImplementedException();
		}

		public Task<FoodDescriptor> Lookup(string barcode, string barcodeType)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}