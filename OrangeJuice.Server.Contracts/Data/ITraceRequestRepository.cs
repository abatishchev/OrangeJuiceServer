using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface ITraceRequestRepository
	{
		Task Add(DateTime timestamp, string url, string httpMethod, string ipAddress, string userAgent);
	}
}