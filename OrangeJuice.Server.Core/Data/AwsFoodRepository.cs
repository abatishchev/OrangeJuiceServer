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
		private readonly IAwsProvider _provider;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;
		private readonly IFilter<FoodDescription> _foodDescriptionFilter;
		private readonly IIdSelector _idSelector;

		public AwsFoodRepository(IAwsProvider provider, IFoodDescriptionFactory foodDescriptionFactory, IFilter<FoodDescription> foodDescriptionFilter, IIdSelector idSelector)
		{
			_provider = provider;
			_foodDescriptionFactory = foodDescriptionFactory;
			_foodDescriptionFilter = foodDescriptionFilter;
			_idSelector = idSelector;
		}

		public async Task<ICollection<FoodDescription>> Search(string title)
		{
			ICollection<XElement> items = await _provider.SearchItems(title);
			ICollection<string> ids = items.Select(_idSelector.GetId).ToArray();

			Task<ICollection<XElement>> attributes = _provider.LookupAttributes(ids);
			Task<ICollection<XElement>> images = _provider.LookupImages(ids);

			return await Task.WhenAll(attributes, images)
							 .ContinueWith(t => CreateFoodDescriptions(ids, t.Result[0], t.Result[1]));
		}

		private ICollection<FoodDescription> CreateFoodDescriptions(IEnumerable<string> ids, IEnumerable<XElement> attrributs, IEnumerable<XElement> images)
		{
			return ids.Zip(
				Enumerable.Zip(attrributs, images, (a, i) => new { Attributes = a, Images = i }),
				(id, x) => _foodDescriptionFactory.Create(id, x.Attributes, x.Images))
					  .Where(_foodDescriptionFilter.Filter)
					  .ToArray();
		}
	}
}