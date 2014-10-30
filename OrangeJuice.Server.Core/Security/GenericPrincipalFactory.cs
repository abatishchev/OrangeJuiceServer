using System.Security.Principal;

namespace OrangeJuice.Server.Security
{
	public sealed class GenericPrincipalFactory : Factory.IFactory<IPrincipal, string>
	{
		public IPrincipal Create(string param)
		{
			return new GenericPrincipal(WindowsIdentity.GetCurrent(), new string[0]);
		}
	}
}