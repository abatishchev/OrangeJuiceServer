using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProductService : IProductService
	{
		#region Fields
		private readonly IAwsProductProvider _awsProvider;
		#endregion

		#region Ctor
		public AwsProductService(IAwsProductProvider awsProvider)
		{
			_awsProvider = awsProvider;
		}
		#endregion

		#region IProductService members
		public Task<ProductDescriptor> Get(Guid productId)
		{
			throw new NotSupportedException();
		}

		public Task<ProductDescriptor> Search(string barcode, BarcodeType barcodeType)
		{
			return _awsProvider.Search(barcode, barcodeType);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
		}
		#endregion
	}
}