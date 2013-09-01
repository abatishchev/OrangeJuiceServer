namespace OrangeJuice.Server.Services
{
	public interface IQuerySigner
	{
		string SignQuery(string query);
	}
}