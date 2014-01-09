using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsProvider : IAwsProvider
	{
		#region Fields
		private readonly IAwsClient _client;
		#endregion

		#region Ctor
		public AwsProvider(IAwsClient client)
		{
			_client = client;
		}
		#endregion

		#region IAwsProvider members
		public Task<ICollection<XElement>> SearchItems(string title)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemSearch" },
				{ "SearchIndex", "Grocery" },
				{ "ResponseGroup", "Small" },
				{ "Title", title }
			};

			return _client.GetItems(args);
		}

		public Task<ICollection<XElement>> LookupAttributes(IEnumerable<string> ids)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "ResponseGroup", "ItemAttributes" },
				{ "ItemId", String.Join(",", ids) }
			};

			return _client.GetItems(args);
		}

		public Task<ICollection<XElement>> LookupImages(IEnumerable<string> ids)
		{
			var args = new Dictionary<string, string>
			{
				{ "Operation", "ItemLookup" },
				{ "ResponseGroup", "Images" },
				{ "ItemId", String.Join(",", ids) }
			};

			return _client.GetItems(args);
		}
		#endregion
	}
}