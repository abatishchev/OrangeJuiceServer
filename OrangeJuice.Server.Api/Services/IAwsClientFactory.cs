namespace OrangeJuice.Server.Api.Services
{
	public interface IAwsClientFactory
	{
		AwsClient Create();
	}
}