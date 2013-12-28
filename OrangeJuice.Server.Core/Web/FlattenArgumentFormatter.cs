using System;
using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server.Web
{
	public class FlattenArgumentFormatter : IArgumentFormatter
	{
		private readonly IUrlEncoder _urlEncoder;

		public FlattenArgumentFormatter(IUrlEncoder urlEncoder)
		{
			_urlEncoder = urlEncoder;
		}

		public string FormatArgs(IDictionary<string, string> args)
		{
			return String.Join("&",
				args.Select(p => String.Format("{0}={1}",
					_urlEncoder.Encode(p.Key),
					_urlEncoder.Encode(p.Value))));
		}
	}
}