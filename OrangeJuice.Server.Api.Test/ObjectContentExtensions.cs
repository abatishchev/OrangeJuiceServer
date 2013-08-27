using System;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Test
{
	public static class ObjectContentExtensions
	{
		public static Exception GetException(this HttpContent content)
		{
			if (content == null)
				throw new ArgumentNullException("content");

			var error = content.GetValue<System.Web.Http.HttpError>();
			if (error == null)
				throw new InvalidOperationException("Content doesn't contain HttpError");

			if (error.ExceptionType == null)
				throw new InvalidOperationException("ExceptionType is null");

			var type = Type.GetType(error.ExceptionType);
			if (type == null)
				throw new InvalidOperationException("Type not found");

			return (Exception)Activator.CreateInstance(type);
		}

		public static T GetValue<T>(this HttpContent content)
		{
			if (content == null)
				throw new ArgumentNullException("content");

			var objectContent = content as ObjectContent<T>;
			if (objectContent == null)
				throw new InvalidOperationException("Content is not ObjectContent<T>");

			return objectContent.GetValue();
		}

		public static T GetValue<T>(this ObjectContent<T> content)
		{
			return (T)content.Value;
		}
	}
}