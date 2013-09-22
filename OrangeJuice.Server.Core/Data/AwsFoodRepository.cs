using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IAwsProviderFactory _providerFactory;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;
		private readonly IFilter<FoodDescription> _foodDescriptionFilter;

		public AwsFoodRepository(IAwsProviderFactory providerFactory, IFoodDescriptionFactory foodDescriptionFactory, IFilter<FoodDescription> foodDescriptionFilter)
		{
			if (providerFactory == null)
				throw new ArgumentNullException("providerFactory");
			if (foodDescriptionFactory == null)
				throw new ArgumentNullException("foodDescriptionFactory");
			if (foodDescriptionFilter == null)
				throw new ArgumentNullException("foodDescriptionFilter");

			_providerFactory = providerFactory;
			_foodDescriptionFactory = foodDescriptionFactory;
			_foodDescriptionFilter = foodDescriptionFilter;
		}

		public async Task<IEnumerable<FoodDescription>> SearchByTitle(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			IAwsProvider provider = _providerFactory.Create();

			var items = await provider.SearchItem(title);

			var tasks = from item in items
						let id = _foodDescriptionFactory.GetId(item)
						select _foodDescriptionFactory.Create(
							id,
							provider.LookupAttributes(id),
							provider.LookupImages(id));

			return await Task.WhenAll(tasks)
							 .ContinueWith(t => t.Result.Where(_foodDescriptionFilter.Filter));
		}
	}
}