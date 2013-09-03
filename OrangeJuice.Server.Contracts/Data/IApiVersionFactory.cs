using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IApiVersionFactory
	{
		ApiVersion Create();
	}
}