using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace OrangeJuice.Server.Api.Diagnostics
{
	internal sealed class EntityErrorLog : Elmah.SqlErrorLog
	{
		internal const string ConnectionStringNameKey = "connectionStringName";

		private readonly string _connectionStringName;

		public EntityErrorLog(System.Collections.IDictionary config)
			: base(config)
		{
			_connectionStringName = (string)config[ConnectionStringNameKey];
		}

		public override string ConnectionString
		{
			get { return GetProviderPart(_connectionStringName); }
		}

		private static string GetProviderPart(string connectionStringName)
		{
			string entityConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder(entityConnectionString);
			return builder.ProviderConnectionString;
		}
	}
}