using System;
using System.Text;

namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncoder : IUrlEncoder
	{
		/// <summary>
		/// Percent-encodes (URL Encode) according to RFC 3986 as required by Amazon.
		/// </summary>
		/// <remarks>
		/// This is necessary because .NET's HttpUtility.UrlEncode does not encode according to the above standard.
		/// Also, .NET returns lower-case encoding by default and Amazon requires upper-case encoding.
		/// </remarks>
		public string Encode(string url)
		{
			url = System.Web.HttpUtility.UrlEncode(url, Encoding.UTF8);
			url = url.Replace("'", "%27")
					 .Replace("(", "%28")
					 .Replace(")", "%29")
					 .Replace("*", "%2A")
					 .Replace("!", "%21")
					 .Replace("%7e", "~")
					 .Replace("+", "%20");

			StringBuilder sb = new StringBuilder(url);
			for (int i = 0; i < sb.Length; i++)
			{
				if (sb[i] != '%')
					continue;

				if (!Char.IsLetter(sb[i + 1]) && !Char.IsLetter(sb[i + 2]))
					continue;

				sb[i + 1] = Char.ToUpperInvariant(sb[i + 1]);
				sb[i + 2] = Char.ToUpperInvariant(sb[i + 2]);
			}
			return sb.ToString();
		}
	}
}