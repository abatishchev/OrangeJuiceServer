using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;

using FluentAssertions;
using Xunit;

namespace OrangeJuice.Server.Api.Test
{
	public class AttributeRoutingTest
	{
		[Fact]
		public void Every_Method_Of_ApiController_Returning_IHttpActionResult_Should_Be_Decorated_With_RouteAttribute()
		{
			// Arrange
			// Act
			var assembly = Assembly.Load("OrangeJuice.Server.Api");
			var attributes = from t in assembly.GetTypes()
							 where typeof(ApiController).IsAssignableFrom(t)
							 from m in t.GetMethods()
							 where IsTypeOf(m.ReturnType, typeof(IHttpActionResult))
							 select m.GetCustomAttributes<RouteAttribute>().FirstOrDefault();

			// Assert
			attributes.Should().NotContainNulls();
		}

		private static bool IsTypeOf(Type returnType, Type targetType)
		{
			return returnType.IsGenericType ? returnType.GenericTypeArguments[0] == targetType : returnType == targetType;
		}
	}
}