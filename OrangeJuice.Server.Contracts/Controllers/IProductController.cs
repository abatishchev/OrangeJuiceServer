using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Controllers
{
	public interface IProductController
	{
		Task<IHttpActionResult> GetProductId(ProductSearchCriteria searchCriteria);

		Task<IHttpActionResult> GetProductBarcode(BarcodeSearchCriteria searchCriteria);
	}
}