using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityProductRepository : IProductRepository
	{
		#region Fields
		private readonly IProductUnit _productUnit;
		#endregion

		#region Ctor
		public EntityProductRepository(IProductUnit productUnit)
		{
			_productUnit = productUnit;
		}
		#endregion

		#region IProductRepository members
		public IEnumerable<IProduct> Search(string barcode, BarcodeType barcodeType)
		{
			return _productUnit.Search(barcode, barcodeType).ToArray();
		}

		public async Task<Guid> Save(string barcode, BarcodeType barcodeType, string sourceProductId)
		{
			Product product = await _productUnit.Add(barcode, barcodeType, sourceProductId);
			return product.ProductId;
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_productUnit.Dispose();
		}
		#endregion
	}
}