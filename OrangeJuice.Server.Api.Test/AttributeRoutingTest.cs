using System.Linq;
using System.Reflection;
using System.Web.Http;

using FluentAssertions;
using Xunit;

namespace OrangeJuice.Server.Api.Test
{
	public class AttributeRoutingTest
	{
		#region Test methods
		[Fact]
		public void Every_Method_Of_ApiController_Returning_IHttpActionResult_Should_Be_Decorated_With_RouteAttribute()
		{
			// Arrange
			// Act
			var assembly = Assembly.Load("OrangeJuice.Server.Api");
			var attributes = from t in assembly.GetTypes()
							 where typeof(ApiController).IsAssignableFrom(t)
							 from m in t.GetMethods()
							 where m.ReturnType.IsGenericType ?
									   m.ReturnType.GenericTypeArguments[0] == typeof(IHttpActionResult) :
									   m.ReturnType == typeof(IHttpActionResult)
							 select m.GetCustomAttributes<RouteAttribute>().FirstOrDefault();

			// Assert
			attributes.Should().NotContainNulls();
		}
		#endregion
	}
}