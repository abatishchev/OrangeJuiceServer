using System;
using System.Collections.Generic;
using System.Linq;

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
			return SpltParameters(args.Select(a => SplitNameValue(a.Key, a.Value)));
		}

		public string SignQuery(string query, string signature)
		{
			return SpltParameters(new[]
				{
					query,
					SplitNameValue("Signature", signature)
				});
		}

		private string SplitNameValue(string name, string value)
		{
			return String.Format("{0}={1}", name, _urlEncoder.Encode(value));
		}

		private static string SpltParameters(IEnumerable<string> parameters)
		{
			return String.Join("&", parameters);
		}
		#endregion
	}
}