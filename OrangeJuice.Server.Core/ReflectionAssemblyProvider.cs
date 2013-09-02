using System.Reflection;

namespace OrangeJuice.Server
{
	public sealed class ReflectionAssemblyProvider : IAssemblyProvider
	{
		public Assembly GetExecutingAssembly()
		{
			return Assembly.GetExecutingAssembly();
		}
	}
}