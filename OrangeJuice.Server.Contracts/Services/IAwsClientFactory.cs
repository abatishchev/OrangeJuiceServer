namespace OrangeJuice.Server.Services
{
	public interface IAwsClientFactory
	{
		IAwsClient Create();
	}
}