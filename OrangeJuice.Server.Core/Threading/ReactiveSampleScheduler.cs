using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Threading
{
	public sealed class ReactiveSampleScheduler : TaskScheduler
	{
		private readonly Subject<Task> _tasks = new Subject<Task>();

		public ReactiveSampleScheduler(TimeSpan interval)
		{
			_tasks.Sample(interval)
				  .Subscribe(t => TryExecuteTask(t));
		}

		protected override void QueueTask(Task task)
		{
			_tasks.OnNext(task);
		}

		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			return false;
		}

		protected override IEnumerable<Task> GetScheduledTasks()
		{
			yield break;
		}
	}
}