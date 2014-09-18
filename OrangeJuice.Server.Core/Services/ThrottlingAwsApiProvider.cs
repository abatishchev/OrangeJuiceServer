using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Services
{
	public sealed class ThrottlingAwsApiProvider : IAwsApiProvider
	{
		#region Fields
		private readonly IScheduler _scheduler = new EventLoopScheduler();

		private readonly ObservableCollection<Task<string>> _collection = new ObservableCollection<Task<string>>();
		#endregion

		#region Ctor
		public ThrottlingAwsApiProvider(AwsOptions awsOptions)
		{
			Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
					      e => _collection.CollectionChanged += e,
					      e => _collection.CollectionChanged -= e)
					  .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add)
					  .Select(e => e.EventArgs.NewItems.Cast<Task<string>>())
					  .ObserveOn(_scheduler)
					  .Do(x => _scheduler.Schedule(() => Thread.Sleep(awsOptions.RequestLimit)))
					  .Subscribe(Consume);
		}
		#endregion

		#region IAwsApiProvider members
		public Task<string> ScheduleRequest(Task<string> request)
		{
			_collection.Add(request);

			return request;
		}
		#endregion

		#region Methods
		private static void Consume(IEnumerable<Task<string>> tasks)
		{
			var task = tasks.FirstOrDefault();
			if (task != null)
			{
				Task.Run(() => task);
			}
		}
		#endregion
	}
}