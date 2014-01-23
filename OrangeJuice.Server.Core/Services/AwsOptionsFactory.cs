namespace OrangeJuice.Server.Services
{
	public class AwsOptionsFactory : IFactory<AwsOptions>
	{
		internal const string AwsAccess = "AKIAICFWNOWCE42LO7BQ";
		internal const string AwsAssociate = "orang04-20";
		internal const string AwsSecret = "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";

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