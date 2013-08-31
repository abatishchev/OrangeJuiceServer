using System;
using System.Reflection;

using FluentValidation;

using Microsoft.Practices.Unity;

namespace OrangeJuice.Server.Api.Validation.Infrustructure
{
	// TODO: repalce with FluentValidation.Attributes.AttributeValidatorFactory
	public sealed class AttributeValidatorFactory : ValidatorFactoryBase
	{
		private readonly IUnityContainer _container;

		public AttributeValidatorFactory(IUnityContainer container)
		{
			if (container == null)
				throw new ArgumentNullException("container");
			_container = container;
		}

		public override IValidator CreateInstance(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			var validatorAttribute = type.GetCustomAttribute<FluentValidation.Attributes.ValidatorAttribute>();
			if (validatorAttribute == null)
				return null;

			return (IValidator)_container.Resolve(validatorAttribute.ValidatorType);
		}
	}
}