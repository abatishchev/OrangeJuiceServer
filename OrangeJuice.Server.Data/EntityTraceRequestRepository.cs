using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public sealed class EntityTraceRequestRepository : ITraceRequestRepository
	{
		private readonly IModelContext _db;

		public EntityTraceRequestRepository(IModelContext db)
		{
			_db = db;
		}

		public Task Add(DateTime timestamp, string url, string httpMethod, string ipAddress, string userAgent)
		{
			_db.Requests.Add(
				new Request
				{
					Timestamp = timestamp,
					Url = url,
					HttpMethod = httpMethod,
					IpAddress = ipAddress,
					UserAgent = userAgent
				});

			return _db.SaveChangesAsync();
		}
	}
}