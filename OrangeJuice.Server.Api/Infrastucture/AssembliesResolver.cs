using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace OrangeJuice.Server.Api.Infrastucture
{
	internal sealed class AssembliesResolver : IAssembliesResolver
	{
		private readonly ICollection<Assembly> _assemblies;

		public AssembliesResolver(params  Assembly[] assemblies)
		{
			_assemblies = assemblies;
		}

		public ICollection<Assembly> GetAssemblies()
		{
			return _assemblies;
		}
	}
}