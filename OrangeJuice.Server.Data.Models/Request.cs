using System;

namespace OrangeJuice.Server.Data.Models
{
	public class Request
	{
		public Guid RequestId { get; internal set; }

		public DateTime Timestamp { get; set; }

		public string Url { get; set; }

		public string HttpMethod { get; set; }

		public string IpAddress { get; set; }

		public string UserAgent { get; set; }
	}
}