using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class CompositeProductRepository : IProductRepository
	{
		#region Fields
		private readonly IEnumerable<IProductProvider> _providers;
		private readonly IValidator<ProductDescriptor> _validator;
		#endregion

		#region Ctor
		public CompositeProductRepository(IEnumerable<IProductProvider> providers, IValidator<ProductDescriptor> validator)
		{
			_validator = validator;
			_providers = providers;
		}
		#endregion

		#region IProductRepository members
		public async Task<IEnumerable<ProductDescriptor>> Search(string title)
		{
			return await _providers.Select(p => p.SearchTitle(title))
									   .FirstOrDefaultAsync(d => d != null);
		}

		public async Task<ProductDescriptor> Lookup(string barcode, BarcodeType barcodeType)
		{
			// TODO: rework the flow
			// 1. Get product id
			// 2. Check azure blob
			// 3. Search aws
			// 4. Cache in azure blob

			return await _providers.Select(p => p.SearchBarcode(barcode, barcodeType.ToString()))
								   .FirstOrDefaultAsync(d => d != null && _validator.IsValid(d));
		}

		#endregion
	}
}