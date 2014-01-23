using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class ApiVersionFactoryTest
	{
		#region Create
		[TestMethod]
		public void Create_Should_Call_AssemblyProvider_GetExecutingAssembly()
		{
			// Arrange
			var assemblyProviderMock = CreateAssemblyProvider();
			var factory = CreateFactory(assemblyProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			assemblyProviderMock.Verify(p => p.GetExecutingAssembly(), Times.Once);
		}
		#endregion

		#region Helper methods
		private static IFactory<ApiVersion> CreateFactory(IAssemblyProvider assemblyProvider = null)
		{
			return new ApiVersionFactory(assemblyProvider ?? CreateAssemblyProvider().Object);
		}

		private static Mock<IAssemblyProvider> CreateAssemblyProvider()
		{
			var providerMock = new Mock<IAssemblyProvider>();
			providerMock.Setup(p => p.GetExecutingAssembly()).Returns(typeof(ApiVersionFactoryTest).Assembly);
			return providerMock;
		}
		#endregion
	}
}