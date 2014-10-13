using System;
using System.Collections.Specialized;
using System.Linq;

namespace OrangeJuice.Server.Web
{
	public sealed class QueryStringNameValueCollection : NameValueCollection
	{
		#region Fields
		private readonly IUrlEncoder _urlEncoder;
		#endregion

		#region Ctor
		public QueryStringNameValueCollection(NameValueCollection coll, IUrlEncoder urlEncoder)
			: base(coll)
		{
			_urlEncoder = urlEncoder;
		}
		#endregion

		#region Methods
		public override string Get(string name)
		{
			return base.Get(name) ?? String.Empty;
		}

		public override string ToString()
		{
			var args = this.AllKeys
						   .Select(k => new { Key = k, Value = _urlEncoder.Encode(this[k]) })
						   .Select(p => String.Format("{0}={1}", p.Key, p.Value));
			return String.Join("&", args);
		}
		#endregion
	}
}