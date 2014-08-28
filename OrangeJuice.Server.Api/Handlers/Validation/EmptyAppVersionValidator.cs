using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
    public sealed class EmptyAppVersionValidator : IValidator<HttpRequestMessage>
    {
        public bool IsValid(HttpRequestMessage request)
        {
            return true;
        }
    }
}