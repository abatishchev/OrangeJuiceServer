namespace OrangeJuice.Server.Services
{
	public interface IQuerySigner
	{
		string SignQuery(string host, string path, string query);
	}
}