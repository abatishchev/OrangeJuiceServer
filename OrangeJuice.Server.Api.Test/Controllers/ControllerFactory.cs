using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using Moq;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory
	{
		public static T Create<T>(params object[] args) where T : ApiController
		{
			T controller = (T)Activator.CreateInstance(typeof(T), args);
			controller.Request = new HttpRequestMessage();
			controller.Request.Properties["MS_HttpConfiguration"] = new HttpConfiguration();
			return controller;
		}

		public static IModelValidator CreateModelValidator(Func<ModelStateDictionary, bool> isValidFunc = null)
		{
			var modelValidatorMock = new Mock<IModelValidator>(MockBehavior.Strict);
			modelValidatorMock.Setup(v => v.IsValid(It.IsAny<ModelStateDictionary>())).Returns(isValidFunc ?? (s => true));
			return modelValidatorMock.Object;
		}

		public static IDisposable NewContext(IModelValidator modelValidator = null)
		{
			IModelValidator current = ModelValidator.Current;
			return new TestContext(() =>
			{
				ModelValidator.Current = modelValidator ?? CreateModelValidator();
			}, () =>
			{
				ModelValidator.Current = current;
			});
		}
	}
}