using System.Web.Http.ModelBinding;

namespace OrangeJuice.Server.Api.Validation
{
    public interface IModelValidator
    {
        bool IsValid(ModelStateDictionary modelState);
    }
}