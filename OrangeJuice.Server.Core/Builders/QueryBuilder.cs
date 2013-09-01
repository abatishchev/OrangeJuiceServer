using System;
using System.Collections.Generic;
using System.Linq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Builders
{
	public class QueryBuilder : IQueryBuilder
	{
		private readonly IUrlEncoder _urlEncoder;

		public QueryBuilder(IUrlEncoder urlEncoder)
		{
			if (urlEncoder == null)
				throw new ArgumentNullException("urlEncoder");

			_urlEncoder = urlEncoder;
		}

		public string BuildQuery(IDictionary<string, string> dic)
		{
			// Get the canonical query string
			return String.Join("&",
				dic.Select(p => String.Format("{0}={1}",
					_urlEncoder.Encode(p.Key),
					_urlEncoder.Encode(p.Value))));
		}
	}
}