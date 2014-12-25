using System.Collections.Generic;
using System.Linq;
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

		public AssembliesResolver(IEnumerable<Assembly> assemblies)
			: this(assemblies.ToArray())
		{
		}

		public ICollection<Assembly> GetAssemblies()
		{
			return _assemblies;
		}
	}
}