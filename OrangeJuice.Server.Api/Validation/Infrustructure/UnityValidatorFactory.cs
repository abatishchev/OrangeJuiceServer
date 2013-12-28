using System;

using FluentValidation;

using Microsoft.Practices.Unity;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	public sealed class UnityValidatorFactory : ValidatorFactoryBase
	{
		private readonly IUnityContainer _container;

		public UnityValidatorFactory(IUnityContainer container)
		{
			_container = container;
		}

		public override IValidator CreateInstance(Type type)
		{
			return (IValidator)_container.Resolve(type);
		}
	}
}