using System;

using FluentValidation;

using Microsoft.Practices.Unity;

namespace OrangeJuice.Server.Api.Validation
{
	internal sealed class ValidatorFactory : ValidatorFactoryBase
	{
		private readonly IUnityContainer _container;

		public ValidatorFactory(IUnityContainer container)
		{
			if (container == null)
				throw new ArgumentNullException("container");
			_container = container;
		}

		public override IValidator CreateInstance(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			return (IValidator)_container.Resolve(type);
		}
	}
}