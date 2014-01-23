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
		public string BuildQuery(IDictionary<string, string> args)
		{
			return String.Join("&",
				args.Select(p => String.Format("{0}={1}",
					p.Key,
					_urlEncoder.Encode(p.Value))));
		}
		#endregion
	}
}