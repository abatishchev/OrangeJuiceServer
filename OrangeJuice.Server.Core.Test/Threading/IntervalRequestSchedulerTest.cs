using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Reactive.Testing;
using Xunit;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Threading;

namespace OrangeJuice.Server.Test.Threading
{
	public class IntervalRequestSchedulerTest
	{
		[Fact]
		public void ScheduleRequest_Should_Call_Sample_On_Scheduler()
		{
			// Arrange
			var scheduler = new TestScheduler();

			TimeSpan requestLimit = TimeSpan.FromMilliseconds(250);
			IRequestScheduler requestScheduler = new IntervalRequestScheduler(new AwsOptions { RequestLimit = requestLimit }, scheduler);

			// Act
			var tasks = Enumerable.Range(0, 10).Select(CreateRequest)
								  .Select(requestScheduler.ScheduleRequest)
								  .ToArray();

			scheduler.AdvanceBy(requestLimit.Ticks);

			//Assert
			tasks.Count(t => t.IsCompleted).Should().Be(1);
		}

		private static Func<Task<int>> CreateRequest(int i)
		{
			return () => Task.FromResult(i);
		}
	}
}