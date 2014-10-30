using System.Net.Http;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Services
{
	public sealed class UriMakerAdapter<T> : Drum.UriMaker<T>
	{
		public UriMakerAdapter(Drum.UriMakerContext context, IRequestMessageProvider requestMessageProvider)
			: base(context, requestMessageProvider.CurrentMessage ?? new HttpRequestMessage())
		{
		}
	}
}