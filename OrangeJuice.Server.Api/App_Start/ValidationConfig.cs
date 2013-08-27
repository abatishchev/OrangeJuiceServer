using System;
using System.Web.Http.Validation;

using Microsoft.Practices.Unity;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	static class ValidationConfig
	{
		public static void Configure(System.Web.Http.Controllers.ServicesContainer services, IUnityContainer container)
		{
			// TODO: wait for FluentValidation+WebApi intergration/implementation
			// see https://fluentvalidation.codeplex.com/SourceControl/network/forks/havard/webapisupport/contribution/2253

			//services.Add(typeof(ModelValidatorProvider), container.Resolve<ModelValidatorProvider>());
		}
	}
}