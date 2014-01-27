namespace OrangeJuice.Server.Services
{
	public interface IBlobNameResolver
	{
		string Resolve(string blobName);
	}
}