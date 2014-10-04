using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Threading;

namespace OrangeJuice.Server.Test.Threading
{
	[TestClass]
	public class IntervalRequestSchedulerTest
	{
		[TestMethod]
		public void ScheduleRequest_Should_Process_Requests_Not_Faster_Than_AwsOptions_RequestLimit()
		{
			// Arrange
			TimeSpan requestLimit = TimeSpan.FromMilliseconds(1000);

			IRequestScheduler scheduler = new IntervalRequestScheduler(new AwsOptions { RequestLimit = requestLimit });

		    Stopwatch sw = Stopwatch.StartNew();
		    var list = new List<TimeSpan>();

			// Act
		    var tasks = Enumerable.Range(0, 10)
		                          .Select(_ => scheduler.ScheduleRequest(
		                              () =>
		                              {
		                                  list.Add(sw.Elapsed);
		                                  return Task.FromResult(new object());
		                              }));
		    Task.WaitAll(tasks.ToArray());
            sw.Stop();

			//Assert
		    list.Skip(1).Aggregate((x, y) =>
		                           {
		                               return x;
		                           });
		}	
	}
}