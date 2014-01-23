using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IAwsProvider _provider;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;

		public AwsFoodRepository(IAwsProvider provider, IFoodDescriptionFactory foodDescriptionFactory)
		{
			_provider = provider;
			_foodDescriptionFactory = foodDescriptionFactory;
		}

		public async Task<ICollection<FoodDescription>> Search(string title)
		{
			ICollection<XElement> items = await _provider.SearchItems(title);
			return items.Select(i => _foodDescriptionFactory.Create(i))
						.ToArray();
		}
	}
}