using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Container;

// ReSharper disable once CheckNamespace
namespace OrangeJuice.Server.Data
{
	public partial class ModelContainer : IModelContainer
	{
	    public ModelContainer(IConfigurationProvider configurationProvider)
            : base(configurationProvider.GetValue("sql:ConnectionString"))
	    {
	    }
	}
}