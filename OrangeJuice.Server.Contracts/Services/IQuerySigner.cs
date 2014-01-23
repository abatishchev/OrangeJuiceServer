namespace OrangeJuice.Server.Services
{
	public interface IQuerySigner
	{
		string CreateSignature(string host, string path, string query);
	}
}