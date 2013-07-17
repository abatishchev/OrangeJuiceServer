namespace OrangeJuice.Server.Api.Test.Controllers
{
    internal static class ControllerFactory
    {
        public static T Create<T>() where T : System.Web.Http.ApiController, new()
        {
            T controller = new T();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Request.Properties["MS_HttpConfiguration"] = new System.Web.Http.HttpConfiguration();
            return controller;
        }
    }
}