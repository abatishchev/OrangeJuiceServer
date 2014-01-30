using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class CompositeProductRepositoryTest
	{
		#region Search
		[TestMethod]
		public void Search_Should()
		{
			// Arrange

			// Act
			Assert.Inconclusive();

			// Assert
		}
		#endregion

		#region Lookup
		[TestMethod]
		public void Lookup_Should()
		{
			// Arrange

			// Act
			Assert.Inconclusive();

			// Assert
		}
		#endregion

		#region Helper methods
		private static IProductCoordinator CreateCoordinator(IProductRepository productRepository, IProductProvider azureProvider, IProductProvider awsProvider)
		{
			return new CloudProductCoordinator(
				productRepository,
				azureProvider,
				awsProvider);
		}
		#endregion
	}
}