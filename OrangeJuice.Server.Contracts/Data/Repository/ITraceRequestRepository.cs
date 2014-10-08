using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface ITraceRequestRepository
	{
		Task Add(string url, string httpMethod, string ipAddress, string userAgent);
	}
}