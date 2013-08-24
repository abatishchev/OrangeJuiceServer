using System;

using OrangeJuice.Server.Api.Builders;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Services
{
	public sealed class AwsClientFactory
	{
		private readonly AwsOptions _awsOptions;
		private readonly IUrlEncoder _urlEncoder;
		private readonly IDateTimeProvider _dateTimeProvider;

		public AwsClientFactory(AwsOptions awsOptions, IUrlEncoder urlEncoder, IDateTimeProvider dateTimeProvider)
		{
			if (awsOptions == null)
				throw new ArgumentNullException("awsOptions");
			if (urlEncoder == null)
				throw new ArgumentNullException("urlEncoder");
			if (dateTimeProvider == null)
				throw new ArgumentNullException("dateTimeProvider");

			_awsOptions = awsOptions;
			_urlEncoder = urlEncoder;
			_dateTimeProvider = dateTimeProvider;
		}

		public AwsClient Create()
		{
			return new AwsClient(
				new ArgumentBuilder(_awsOptions.AssociateTag),
				new QueryBuilder(_awsOptions.AccessKey, _urlEncoder, _dateTimeProvider),
				new SignatureBuilder(_awsOptions.SecretKey, _urlEncoder));
		}
	}
}