using System;
using System.Net.Http;
using System.Web.Http;

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
    }
}