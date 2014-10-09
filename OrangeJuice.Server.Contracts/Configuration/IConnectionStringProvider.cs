namespace OrangeJuice.Server.Configuration
{
	public interface IConnectionStringProvider
	{
		string GetDefaultConnectionString();
	}
}