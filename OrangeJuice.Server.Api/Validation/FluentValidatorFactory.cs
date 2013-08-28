using System;

using FluentValidation;

using Microsoft.Practices.Unity;

namespace OrangeJuice.Server.Api.Validation
{
	// TODO: tests
	internal sealed class UnityValidatorFactory : ValidatorFactoryBase
	{
		private readonly IUnityContainer _container;

		public UnityValidatorFactory(IUnityContainer container)
		{
			if (container == null)
				throw new ArgumentNullException("container");
			_container = container;
		}

		public override IValidator CreateInstance(Type validatorType)
		{
			if (validatorType == null)
				throw new ArgumentNullException("validatorType");
			return (IValidator)_container.Resolve(validatorType);
		}
	}
}