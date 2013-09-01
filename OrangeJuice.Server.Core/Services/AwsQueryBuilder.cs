using System;
using System.Collections.Generic;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public class AwsQueryBuilder : IQueryBuilder
	{
		private readonly IArgumentBuilder _argumentBuilder;
		private readonly IArgumentFormatter _argumentFormatter;
		private readonly IQuerySigner _querySigner;

		public AwsQueryBuilder(IArgumentBuilder argumentBuilder, IArgumentFormatter argumentFormatter, IQuerySigner querySigner)
		{
			if (argumentBuilder == null)
				throw new ArgumentNullException("argumentBuilder");
			if (argumentFormatter == null)
				throw new ArgumentNullException("argumentFormatter");
			if (querySigner == null)
				throw new ArgumentNullException("querySigner");

			_argumentBuilder = argumentBuilder;
			_argumentFormatter = argumentFormatter;
			_querySigner = querySigner;
		}

		public string BuildUrl(IDictionary<string, string> args)
		{
			args = _argumentBuilder.BuildArgs(args);
			string query = _argumentFormatter.FormatArgs(args);
			return _querySigner.SignQuery(query);
		}
	}
}