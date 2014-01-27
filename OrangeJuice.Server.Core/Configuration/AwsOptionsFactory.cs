namespace OrangeJuice.Server.Configuration
{
	public class AwsOptionsFactory : IFactory<AwsOptions>
	{
		private const string AwsAccess = "AKIAICFWNOWCE42LO7BQ";
		private const string AwsAssociate = "orang04-20";
		private const string AwsSecret = "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";

		public AwsOptions Create()
		{
			return new AwsOptions
			{
				AccessKey = AwsAccess,
				AssociateTag = AwsAssociate,
				SecretKey = AwsSecret
			};
		}
	}
}