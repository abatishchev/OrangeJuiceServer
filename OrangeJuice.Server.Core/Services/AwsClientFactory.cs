using System;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsClientFactory : IAwsClientFactory
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

		// TODO: replace with contrainer-based injection
		public IAwsClient Create()
		{
			return new XmlAwsClient(
				new AwsQueryBuilder(
					new AwsArgumentBuilder(_awsOptions.AccessKey, _awsOptions.AssociateTag, _dateTimeProvider),
					new FlattenArgumentFormatter(_urlEncoder),
					new AwsQuerySigner(_awsOptions.SecretKey, _urlEncoder)),
				new HttpDocumentLoader(),
				new XmlItemProvider(
					new XmlRequestValidator()));
		}
	}
}