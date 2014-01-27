using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class CompositeFoodRepository : IFoodRepository
	{
		#region Fields
		private readonly IFoodProvider[] _foodProviders;
		private readonly IValidator<FoodDescriptor> _validator;
		#endregion

		#region Ctor
		public CompositeFoodRepository(IEnumerable<IFoodProvider> foodProviders, IValidator<FoodDescriptor> validator)
		{
			_validator = validator;
			_foodProviders = foodProviders.ToArray();
		}
		#endregion

		#region IFoodRepository members
		public async Task<IEnumerable<FoodDescriptor>> Search(string title)
		{
			return await _foodProviders.Select(p => p.Search(title))
									   .FirstAsync(d => d != null);
		}

		public async Task<FoodDescriptor> Lookup(string barcode, BarcodeType barcodeType)
		{
			return await _foodProviders.Select(p => p.Lookup(barcode, barcodeType.ToString()))
									   .FirstAsync(d => d != null && _validator.IsValid(d));
		}

		#endregion
	}
}