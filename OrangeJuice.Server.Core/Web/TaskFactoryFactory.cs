using System.Threading;
using System.Threading.Tasks;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Threading;

namespace OrangeJuice.Server.Web
{
	public sealed class TaskFactoryFactory : Factory.IFactory<TaskFactory>
	{
		private readonly AwsOptions _awsOptions;

		public TaskFactoryFactory(AwsOptions awsOptions)
		{
			_awsOptions = awsOptions;
		}

		public TaskFactory Create()
		{
			return new TaskFactory(
				CancellationToken.None,
				TaskCreationOptions.DenyChildAttach,
				TaskContinuationOptions.None,
				new ReactiveSampleScheduler(_awsOptions.RequestLimit));
		}
	}
}