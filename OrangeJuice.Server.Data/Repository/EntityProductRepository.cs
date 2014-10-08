using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Context;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityProductRepository : IProductRepository
	{
		#region Fields
		private readonly IModelContext _db;
		#endregion

		#region Ctor
		public EntityProductRepository(IModelContext db)
		{
			_db = db;
		}
		#endregion

		#region IProductRepository members
		public IEnumerable<IProduct> Search(string barcode, BarcodeType barcodeType)
		{
			return _db.Products.Where(p => p.Barcode == barcode && p.BarcodeType == barcodeType);
		}

		public async Task<Guid> Save(string barcode, BarcodeType barcodeType, string sourceProductId)
		{
			Product product = _db.Products.Add(
				new Product
				{
					Barcode = barcode,
					BarcodeType = barcodeType,
					SourceProductId = sourceProductId
				});

			await _db.SaveChangesAsync();

			return product.ProductId;
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_db.Dispose();
		}
		#endregion
	}
}