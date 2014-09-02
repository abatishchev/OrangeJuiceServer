using System.Collections.Specialized;



namespace OrangeJuice.Server.Web

{

	public sealed class HttpUtility
	{

		public static NameValueCollection ParseQueryString(string query, IUrlEncoder urlEncoder)

		{

			var coll = System.Web.HttpUtility.ParseQueryString(query);
			return new QueryStringNameValueCollection(
coll, urlEncoder);

		}

	}

}