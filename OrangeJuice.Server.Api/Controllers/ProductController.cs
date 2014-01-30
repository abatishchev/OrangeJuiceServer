using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class ProductController : ApiController
	{
		#region Fields
		private readonly IProductCoordinator _productCoordinator;
		#endregion

		#region Ctor
		public ProductController(IProductCoordinator productCoordinator)
		{
			_productCoordinator = productCoordinator;
		}
		#endregion

		#region HTTP methods
		/// <summary>
		/// Searches for product by barcode
		/// </summary>
		/// <returns>Single product descriptor</returns>
		/// <url>POST /api/product</url>
		//[ActionName("barcode")]
		public async Task<IHttpActionResult> GetProduct(BarcodeSearchCriteria searchCriteria)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var descriptor = await _productCoordinator.Search(searchCriteria.Barcode, searchCriteria.BarcodeType);
			return Ok(descriptor);
		}
		#endregion
	}
}