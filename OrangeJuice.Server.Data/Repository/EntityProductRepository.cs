using System;
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
		public async Task<IProduct> Search(string barcode, BarcodeType barcodeType)
		{
			return await _productUnit.Get(barcode, barcodeType);
		}

		public async Task<Guid> Save(string barcode, BarcodeType barcodeType)
		{
			Product product = await _productUnit.Add(barcode, barcodeType);
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