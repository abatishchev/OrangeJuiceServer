using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Web
{
	public sealed class EncodedQueryBuilder : IQueryBuilder
	{
		#region Fields
		private readonly IUrlEncoder _urlEncoder;
		#endregion

		#region Ctor
		public EncodedQueryBuilder(IUrlEncoder urlEncoder)
		{
			_urlEncoder = urlEncoder;
		}
		#endregion

		#region IQueryBuilder members
		public string BuildQuery(IEnumerable<KeyValuePair<string, string>> args)
		{
			var coll = HttpUtility.ParseQueryString(String.Empty, _urlEncoder);
			foreach (var arg in args)
			{
				coll.Add(arg.Key, arg.Value);
			}
			return coll.ToString();
		}

		public string SignQuery(string query, string signature)
		{
			var coll = HttpUtility.ParseQueryString(query, _urlEncoder);
			coll.Add("Signature", signature);
			return coll.ToString();
		}
		#endregion
	}
}