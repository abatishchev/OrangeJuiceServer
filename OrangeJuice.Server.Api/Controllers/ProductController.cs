using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class ProductController : ApiController
	{
		#region Fields
		private readonly IProductRepository _productRepository;
		#endregion

		#region Ctor
		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}
		#endregion

		// TODO: change to GET
		#region HTTP methods
		/// <summary>
		/// Searches for product by text
		/// </summary>
		/// <returns>Collection of product descriptors</returns>
		/// <url>POST /api/product</url>
		[ActionName("title")]
		public async Task<IHttpActionResult> PostTitle(TitleSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var descriptors = await _productRepository.Search(searchCriteria.Title);
			return Ok(descriptors.ToArray());
		}

		/// <summary>
		/// Searches for product by barcode
		/// </summary>
		/// <returns>Single product descriptor</returns>
		/// <url>POST /api/product</url>
		[ActionName("barcode")]
		public async Task<IHttpActionResult> PostBarcode(BarcodeSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var descriptor = await _productRepository.Lookup(searchCriteria.Barcode, searchCriteria.BarcodeType);
			return Ok(descriptor);
		}
		#endregion
	}
}