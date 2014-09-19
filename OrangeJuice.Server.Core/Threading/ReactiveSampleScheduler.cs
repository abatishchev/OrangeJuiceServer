using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Threading
{
	// TODO: dispose?
	public sealed class ReactiveSampleScheduler : TaskScheduler, IDisposable
	{
		#region Fields
		private readonly Subject<Task> _tasks = new Subject<Task>();
		private readonly IDisposable _observable;
		#endregion

		#region Ctor
		public ReactiveSampleScheduler(TimeSpan interval)
		{
			_observable = _tasks.Sample(interval)
				.Subscribe(t => TryExecuteTask(t));
		}
		#endregion

		#region TaskScheduler members
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

		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_tasks.Dispose();
			_observable.Dispose();
		}
		#endregion
	}
}