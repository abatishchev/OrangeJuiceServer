using System.Threading.Tasks;

namespace OrangeJuice.Server.Services
{
	public interface IAwsApiProvider
	{
		Task<string> ScheduleRequest(Task<string> request);
	}
}