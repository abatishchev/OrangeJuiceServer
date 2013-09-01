namespace OrangeJuice.Server.Builders
{
	public interface ISignatureBuilder
	{
		string SignQuery(string query);
	}
}