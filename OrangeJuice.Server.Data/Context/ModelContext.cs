using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Context;

// ReSharper disable once CheckNamespace
namespace OrangeJuice.Server.Data
{
	public partial class ModelContext : IModelContext
	{
		public ModelContext(IConnectionStringProvider connectionStringProvider)
			: base(connectionStringProvider.GetDefaultConnectionString())
		{
		}
	}
}