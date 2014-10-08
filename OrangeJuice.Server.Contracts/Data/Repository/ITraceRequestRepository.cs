namespace OrangeJuice.Server.Data.Repository
{
	public interface ITraceRequestRepository
	{
		void Add(string url, string method, string ipAddress, string userAgent);
	}
}