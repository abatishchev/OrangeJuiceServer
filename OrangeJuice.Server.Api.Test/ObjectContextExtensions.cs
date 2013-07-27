using System;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Test
{
	public static class ObjectContextExtensions
	{
		public static T GetValue<T>(this HttpContent context)
		{
			ObjectContent<T> objectContent = context as ObjectContent<T>;
			return objectContent != null ? objectContent.GetValue() : default(T);
		}

		public static T GetValue<T>(this ObjectContent context)
		{
			ObjectContent<T> genericContext = context as ObjectContent<T>;
			return genericContext != null ? genericContext.GetValue() : default(T);
		}

		public static T GetValue<T>(this ObjectContent<T> context)
		{
			return (T)context.Value;
		}
	}
}