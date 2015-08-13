using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Table;

namespace OrangeJuice.Server.Services
{
	public interface IAzureClient
	{
		Task<string> GetBlobFromContainer(string containerName, string fileName);

		Task<string[]> GetBlobsFromContainer(string containerName);

		Task PutBlobToContainer(string containerName, string fileName, string content);

		Task<Uri> GetBlobUrl(string containerName, string fileName);

		Task<T[]> GetEntitiesFromTable<T>(string tableName)
			where T : ITableEntity, new();

		Task<IList<TableResult>> PutEntitiesToTable<T>(string tableName, IEnumerable<T> entities)
			where T : ITableEntity;
	}
}