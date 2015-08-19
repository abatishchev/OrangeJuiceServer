using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using BarcodeType = Ab.Amazon.Data.BarcodeType;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
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

		public Task<Product[]> Search(string barcode, BarcodeType barcodeType)
		{
			return _db.Products.Where(p => p.Barcode == barcode && p.BarcodeType == barcodeType).ToArrayAsync();
		}
		#endregion
	}
}