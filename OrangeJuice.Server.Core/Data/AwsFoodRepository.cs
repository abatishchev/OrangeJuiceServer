using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IFactory<IAwsProvider> _providerFactory;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;
		private readonly IFilter<FoodDescription> _foodDescriptionFilter;

		public AwsFoodRepository(IFactory<IAwsProvider> providerFactory, IFoodDescriptionFactory foodDescriptionFactory, IFilter<FoodDescription> foodDescriptionFilter)
		{
			_providerFactory = providerFactory;
			_foodDescriptionFactory = foodDescriptionFactory;
			_foodDescriptionFilter = foodDescriptionFilter;
		}

		public async Task<ICollection<FoodDescription>> SearchByTitle(string title)
		{
			IAwsProvider provider = _providerFactory.Create();

			ICollection<XElement> items = await provider.SearchItems(title);
			ICollection<string> ids = items.Select(_foodDescriptionFactory.GetId).ToArray(); // TODO: move GetId inside provider?

			Task<ICollection<XElement>> attributes = provider.LookupAttributes(ids);
			Task<ICollection<XElement>> images = provider.LookupImages(ids);

			return await Task.WhenAll(attributes, images)
							 .ContinueWith(t => Enumerable.Zip(t.Result[0], t.Result[1], _foodDescriptionFactory.Create)
														  .Where(_foodDescriptionFilter.Filter)
														  .ToArray());
		}
	}
}