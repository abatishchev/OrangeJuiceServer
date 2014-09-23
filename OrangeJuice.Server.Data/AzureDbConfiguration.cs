﻿using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace OrangeJuice.Server.Data
{
	public sealed class AzureDbConfiguration : DbConfiguration
	{
		public AzureDbConfiguration()
		{
			SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
		}
	}
}