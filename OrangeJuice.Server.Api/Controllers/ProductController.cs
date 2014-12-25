using System;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Controllers;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Controllers
{
	[Authorize]
	public sealed class ProductController : ApiController
	{
		#region Fields
		private readonly IProductService _productService;
		#endregion

		#region Ctor
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		#endregion

		#region HTTP methods
		[Route("api/product/id")]
		public async Task<IHttpActionResult> GetProductId([FromUri]ProductSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			var descriptor = await _productService.Get(searchCriteria.ProductId);
			if (descriptor == null)
				return this.NoContent();

			return Ok(descriptor);
		}

		[Route("api/product/barcode")]
		public async Task<IHttpActionResult> GetProductBarcode([FromUri]BarcodeSearchCriteria searchCriteria)
		{
			if (searchCriteria == null)
				throw new ArgumentNullException();

			var descriptor = await _productService.Search(searchCriteria.Barcode, searchCriteria.BarcodeType);
			if (descriptor == null)
				return this.NoContent();

			return Ok(descriptor);
		}
		#endregion
	}
}