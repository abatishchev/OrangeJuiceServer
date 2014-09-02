using System;
using System.Collections.Generic;

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


	    	public override string ToString()

	    	{

	    		var args = base.AllKeys

	    			       .Select(k => new KeyValuePair<string, string>(k, _urlEncoder.Encode(this[k])))

				       .Select(p => String.Format("{0}={1}", p.Key, p.Value));

	    		return String.Join("&", args);

	    	}

	    	#endregion
	}

}