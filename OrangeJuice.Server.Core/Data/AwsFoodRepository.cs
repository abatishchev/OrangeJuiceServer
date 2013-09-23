﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

			var items = await provider.SearchItems(title);
			var ids = items.Select(_foodDescriptionFactory.GetId).ToArray(); // TODO: move GetId inside provider?

			var attributes = provider.LookupAttributes(ids);
			var images = provider.LookupImages(ids);

			return await Task.WhenAll(attributes, images)
							 .ContinueWith(t => Enumerable.Zip(t.Result.First(), t.Result.Last(), _foodDescriptionFactory.Create)
														  .Where(_foodDescriptionFilter.Filter));
		}
	}
}