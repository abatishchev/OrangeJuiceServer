using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		#region Fields
		private readonly IAwsProvider _provider;
		private readonly IFoodDescriptionFactory _factory;
		#endregion

		#region Ctor
		public AwsFoodRepository(IAwsProvider provider, IFoodDescriptionFactory factory)
		{
			_provider = provider;
			_factory = factory;
		}
		#endregion

		#region IFoodRepository members
		public async Task<ICollection<FoodDescription>> SearchByTitle(string title)
		{
			ICollection<XElement> items = await _provider.SearchItems(title);
			return items.Select(i => _factory.Create(i))
						.ToArray();
		}

		public async Task<FoodDescription> SearchByBarcode(string barcode)
		{
			throw new System.NotImplementedException();
		}
		#endregion

	}
}