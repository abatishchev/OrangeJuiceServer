using System;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class ProductController : ApiController
	{
		#region Fields
		private readonly IProductManager _productManager;
		#endregion

		#region Ctor
		public ProductController(IProductManager productManager)
		{
			_productManager = productManager;
		}
		#endregion

		#region HTTP methods
		[Route("api/product/id")]
		[Route("api/product/id/{productId}")]
		public async Task<IHttpActionResult> GetProductId([FromUri] ProductSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			var descriptor = await _productManager.Get(searchCriteria.ProductId);
			if (descriptor == null)
				return NotFound();

			return Ok(descriptor);
		}

		[Route("api/product/barcode")]
		[Route("api/product/barcode/{barcodeType}/{barcode}")]
		public async Task<IHttpActionResult> GetProductBarcode([FromUri] BarcodeSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			var descriptor = await _productManager.Search(searchCriteria.Barcode, searchCriteria.BarcodeType);
			if (descriptor == null)
				return NotFound();

			return Ok(descriptor);
		}
		#endregion

		#region Methods
		protected override void Dispose(bool disposing)
		{
			_productManager.Dispose();

			base.Dispose(disposing);
		}
		#endregion
	}
}