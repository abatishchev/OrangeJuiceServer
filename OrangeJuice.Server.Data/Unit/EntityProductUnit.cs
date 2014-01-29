using System.Data.Entity;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Container;

namespace OrangeJuice.Server.Data.Unit
{
	public sealed class EntityProductUnit : IProductUnit
	{
		#region Fields
		private readonly IModelContainer _container;
		#endregion

		#region Ctor
		public EntityProductUnit(IModelContainer container)
		{
			_container = container;
		}
		#endregion

		#region IProductUnit members
		public Task<Product> Get(string barcode, BarcodeType barcodeType)
		{
			return _container.Products.FirstOrDefaultAsync(p => p.Barcode == barcode && p.BarcodeType == barcodeType);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_container.Dispose();
		}
		#endregion
	}
}