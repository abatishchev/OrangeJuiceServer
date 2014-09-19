using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Threading
{
	public interface IRequestScheduler : IDisposable
	{
		Task<T> ScheduleRequest<T>(Func<Task<T>> request);
	}
}