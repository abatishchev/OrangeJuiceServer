using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncoder : IUrlEncoder
	{
		#region FIelds
		private static readonly Lazy<IDictionary<string, string>> _charDir = new Lazy<IDictionary<string, string>>(CreateCharacterDictionary);

		private readonly IEnumerable<Func<string, string>> _encodingSteps;
		#endregion

		#region Constructors
		public PercentUrlEncoder()
			: this(GetDefaultEncodingSteps())
		{

		}

		internal PercentUrlEncoder(IEnumerable<Func<string, string>> encodingSteps)
		{
			_encodingSteps = encodingSteps;
		}
		#endregion

		#region IUrlEncoder members
		/// <summary>
		/// Percent-encodes according to RFC 3986 as required by Amazon
		/// </summary>
		/// <remarks>
		/// This is necessary because .NET's HttpUtility.UrlEncode does not encode according to the above standard.
		/// Also it returns lower-case encoding by default and Amazon requires upper-case encoding.
		/// </remarks>
		public string Encode(string url)
		{
			return _encodingSteps.Aggregate(url, (s, func) => func(s));
		}
		#endregion

		#region Methods
		internal static IDictionary<string, string> CreateCharacterDictionary()
		{
			return new Dictionary<string, string>
			{
				{ "'", "%27" },
				{ "(", "%28" },
				{ ")", "%29" },
				{ "*", "%2A" },
				{ "!", "%21" },
				{ "~", "%7E" },
				{ "+", "%20" },
			};
		}

		private static IEnumerable<Func<string, string>> GetDefaultEncodingSteps()
		{
			yield return HttpUtility.UrlEncode;
			yield return PercentEncode;
			yield return UpperCaseEncoding;
		}

		internal static string PercentEncode(string url)
		{
			return _charDir.Value.Aggregate(url, (str, pair) => str.Replace(pair.Key, pair.Value));
		}

		internal static string UpperCaseEncoding(string url)
		{
			return Regex.Replace(url, "%([a-z0-9]{2})", m => '%' + m.Groups[1].Value.ToUpperInvariant());
		}
		#endregion
	}
}