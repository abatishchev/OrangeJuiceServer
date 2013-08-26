using System.Web.Mvc;

using Newtonsoft.Json;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class JsonDotNetResult : ActionResult
	{
		private readonly object _obj;
		private readonly Formatting _formatting;

		public JsonDotNetResult(object obj, Formatting formatting = Formatting.Indented)
		{
			_obj = obj;
			_formatting = formatting;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.AddHeader("content-type", "application/json");
			context.HttpContext.Response.Write(JsonConvert.SerializeObject(_obj, _formatting));
		}
	}
}