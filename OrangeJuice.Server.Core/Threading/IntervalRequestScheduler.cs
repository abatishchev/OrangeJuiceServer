using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Threading
{
	public sealed class IntervalRequestScheduler : IRequestScheduler
	{
		#region Fields
		private readonly Subject<Action> _requests = new Subject<Action>();
		private readonly IDisposable _observable;
		#endregion

		#region Ctor
		public IntervalRequestScheduler(AwsOptions awsOptions, IScheduler scheduler)
		{
			_observable = _requests.Select(i => Observable.Empty<Action>()
			                                              .Delay(awsOptions.RequestLimit)
			                                              .StartWith(i))
			                       .Concat()
			                       .ObserveOn(scheduler)
			                       .Subscribe(action => action());
		}
		#endregion

		#region IRequestScheduler members
		public Task<T> ScheduleRequest<T>(Func<Task<T>> request)
		{
			var tcs = new TaskCompletionSource<T>();
			_requests.OnNext(async () =>
			{
				try
				{
					T result = await request();
					tcs.SetResult(result);
				}
				catch (Exception ex)
				{
					tcs.SetException(ex);
				}
			});
			return tcs.Task;
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_requests.Dispose();
			_observable.Dispose();
		}
		#endregion
	}
}
