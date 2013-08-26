﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IAwsClientFactory _awsClientFactory;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;

		public AwsFoodRepository(IAwsClientFactory awsClientFactory, IFoodDescriptionFactory foodDescriptionFactory)
		{
			if (awsClientFactory == null)
				throw new ArgumentNullException("awsClientFactory");
			if (foodDescriptionFactory == null)
				throw new ArgumentNullException("foodDescriptionFactory");

			_awsClientFactory = awsClientFactory;
			_foodDescriptionFactory = foodDescriptionFactory;
		}

		public async Task<FoodDescription[]> SearchByTitle(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			IAwsClient apiClient = _awsClientFactory.Create();
			var ids = await apiClient.SearchItem(title);

			var tasks = ids.Select(id => _foodDescriptionFactory.Create(
				id,
				apiClient.LookupAttributes(id),
				apiClient.LookupImages(id)));
			return await Task.WhenAll(tasks)
							 .ContinueWith(t => t.Result);
		}
	}
}