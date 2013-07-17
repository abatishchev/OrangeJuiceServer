using System;

namespace OrangeJuice.Server.Api.Validation
{
    internal sealed class ModelValidatorContext : IDisposable
    {
        private readonly IModelValidator _currentContext;

        public ModelValidatorContext(IModelValidator newContext)
        {
            _currentContext = ModelValidator.Current;
            ModelValidator.Current = newContext;
        }

        public void Dispose()
        {
            ModelValidator.Current = _currentContext;
        }
    }
}