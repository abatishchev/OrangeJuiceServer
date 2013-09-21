namespace OrangeJuice.Server.Services
{
	public interface IAwsProviderFactory
	{
		IAwsProvider Create();
	}
}