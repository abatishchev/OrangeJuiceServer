namespace OrangeJuice.Server.Configuration
{
	public interface IEnvironmentProvider
	{
		string GetCurrentEnvironment();
	}
}