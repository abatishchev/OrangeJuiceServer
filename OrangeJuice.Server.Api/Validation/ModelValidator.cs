using System.Web.Http.ModelBinding;

namespace OrangeJuice.Server.Api.Validation
{
    public sealed class ModelValidator : IModelValidator
    {
        public static IModelValidator Current = new ModelValidator();

        public bool IsValid(ModelStateDictionary modelState)
        {
            return modelState.IsValid;
        }
    }
}