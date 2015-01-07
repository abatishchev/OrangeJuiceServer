using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace OrangeJuice.Server.Api.Infrastucture
{
	internal class HttpControllerSelector : DefaultHttpControllerSelector
	{
		private readonly Lazy<IDictionary<string, ILookup<string, Type>>> _controllerTypeCache;
		private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;

		public HttpControllerSelector(HttpConfiguration configuration)
			: base(configuration)
		{
			_controllerTypeCache = new Lazy<IDictionary<string, ILookup<string, Type>>>(() =>
				InitializeCache(configuration));
			_controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(() =>
				new ConcurrentDictionary<string, HttpControllerDescriptor>(InitializeControllerInfoCache(configuration)));
		}

		private static IDictionary<string, ILookup<string, Type>> InitializeCache(HttpConfiguration configuration)
		{
			IAssembliesResolver assembliesResolver = configuration.Services.GetAssembliesResolver();
			IHttpControllerTypeResolver controllersResolver = configuration.Services.GetHttpControllerTypeResolver();

			var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
			var groupedByName = controllerTypes.GroupBy(
				t => t.Name.Substring(0, t.Name.Length - ControllerSuffix.Length),
				StringComparer.OrdinalIgnoreCase);

			return groupedByName.ToDictionary(
				g => g.Key,
				g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
				StringComparer.OrdinalIgnoreCase);
		}

		private IEnumerable<KeyValuePair<string, HttpControllerDescriptor>> InitializeControllerInfoCache(HttpConfiguration configuration)
		{
			return from controllerTypeGroup in _controllerTypeCache.Value
				   let controllerType = controllerTypeGroup.Value.First().First()
				   select new KeyValuePair<string, HttpControllerDescriptor>(
					   controllerTypeGroup.Key,
					   new HttpControllerDescriptor(configuration, controllerTypeGroup.Key, controllerType));
		}

		public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
		{
			return _controllerInfoCache.Value.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
		}
	}
}