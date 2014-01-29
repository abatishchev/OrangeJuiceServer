using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;
using IProductDataRepository = OrangeJuice.Server.Data.Repository.IProductRepository;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class CompositeProductRepositoryTest
	{
		#region Search
		[TestMethod]
		public async Task Search_Should()
		{
			// Arrange

			// Act
			Assert.Inconclusive();

			// Assert
		}
		#endregion

		#region Lookup
		[TestMethod]
		public async Task Lookup_Should()
		{
			// Arrange

			// Act
			Assert.Inconclusive();

			// Assert
		}
		#endregion

		#region Helper methods
		private static IProductRepository CreateRepository(IProductDataRepository dataRepository, IProductProvider azureProvider, IProductProvider awsProvider)
		{
			return new CompositeProductRepository(
				dataRepository ?? CreateDataRepository(),
				azureProvider,
				awsProvider);
		}

		private static IProductDataRepository CreateDataRepository()
		{
			var repositoryMock = new Mock<IProductDataRepository>();
			return repositoryMock.Object;
		}
		#endregion
	}
}