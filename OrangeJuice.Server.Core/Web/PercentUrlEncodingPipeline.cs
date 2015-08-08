using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncodingPipeline : GenericPipeline<string>
	{
		#region Fields
		private static readonly IDictionary<string, string> CharDictionary = new Dictionary<string, string>
		{
			{ "'", "%27" },
			{ "(", "%28" },
			{ ")", "%29" },
			{ "*", "%2A" },
			{ "!", "%21" },
			{ "~", "%7E" },
			{ "+", "%20" },
		};
		#endregion

		#region ObjectPipeline members
		/// <summary>
		/// Percent-encodes according to RFC 3986 as required by Amazon
		/// </summary>
		/// <remarks>
		/// This is necessary because .NET's HttpUtility.UrlEncode does not encode according to the above standard.
		/// Also it returns lower-case encoding by default and Amazon requires upper-case encoding.
		/// </remarks>
		protected override IEnumerable<IPipelineFilter<string>> GetFilters()
		{
			yield return PipelineFilter.Create((string s) => System.Web.HttpUtility.UrlEncode(s));
			yield return PipelineFilter.Create((string s) => PercentEncode(s));
			yield return PipelineFilter.Create((string s) => ToUpperCase(s));
		}
		#endregion

		#region Methods
		internal static string PercentEncode(string url)
		{
			return CharDictionary.Aggregate(url, (str, pair) => str.Replace(pair.Key, pair.Value));
		}

		internal static string ToUpperCase(string url)
		{
			return Regex.Replace(url, "%([a-z0-9]{2})", m => '%' + m.Groups[1].Value.ToUpperInvariant());
		}
		#endregion
	}
}