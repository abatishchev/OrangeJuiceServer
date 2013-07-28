using System.Data.Entity.Core.EntityClient;

namespace OrangeJuice.Server.Api.Diagnostics
{
	public class EntityErrorLog : Elmah.SqlErrorLog
	{
		private readonly string _connectionStringName;

		public EntityErrorLog(System.Collections.IDictionary config)
			: base(config)
		{
			_connectionStringName = (string)config["connectionStringName"];
		}

		public override string ConnectionString
		{
			get
			{
				string entityConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
				EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder(entityConnectionString);
				return builder.ProviderConnectionString;
			}
		}
	}
}