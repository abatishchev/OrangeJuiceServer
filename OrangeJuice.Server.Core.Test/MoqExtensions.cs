using Moq.Language.Flow;

using System.Threading.Tasks;

namespace OrangeJuice.Server.Test
{
	public static class MoqExtensions
	{
		public static IReturnsResult<TMock> ReturnsAsync<TMock, TResult>(this ISetup<TMock, Task<TResult>> setup, TResult value) where TMock : class
		{
			return setup.Returns(Task.FromResult(value));
		}
	}
}