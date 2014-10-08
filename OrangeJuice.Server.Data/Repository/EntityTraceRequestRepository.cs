using System.Threading.Tasks;

using OrangeJuice.Server.Data.Context;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityTraceRequestRepository : ITraceRequestRepository
	{
		private readonly IModelContext _db;

		public EntityTraceRequestRepository(IModelContext db)
		{
			_db = db;
		}

		public Task Add(string url, string httpMethod, string ipAddress, string userAgent)
		{
			_db.Requests.Add(
				new Request
				{
					Url = url,
					HttpMethod = httpMethod,
					IpAddress = ipAddress,
					UserAgent = userAgent
				});

			return _db.SaveChangesAsync();
		}
	}
}