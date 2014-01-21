using System.Collections.Generic;
using System.Collections.Specialized;

namespace OrangeJuice.Server.Web
{
	public class FlattenArgumentFormatter : IArgumentFormatter
	{
		private readonly IUrlEncoder _urlEncoder;

		public FlattenArgumentFormatter(IUrlEncoder urlEncoder)
		{
			_urlEncoder = urlEncoder;
		}

		public NameValueCollection FormatArgs(IDictionary<string, string> args)
		{
			NameValueCollection collection = new NameValueCollection(args.Count);
			foreach (var p in args)
			{
				collection.Add(p.Key, _urlEncoder.Encode(p.Value));
			}
			return collection;
		}
	}
}