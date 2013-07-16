using System.Web.Http.ModelBinding;

namespace OrangeJuice.Server.Api.Validation
{
    public sealed class ModelValidator : IModelValidator
    {
        public bool IsValid(ModelStateDictionary modelState)
        {
            return modelState.IsValid;
        }
    }
}