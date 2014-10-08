using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Context;

// ReSharper disable once CheckNamespace
namespace OrangeJuice.Server.Data
{
	public partial class ModelContext : IModelContext
	{
		public ModelContext(IConfigurationProvider configurationProvider)
			: base(configurationProvider.GetValue("sql:ConnectionString"))
		{
		}
	}
}