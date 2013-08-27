using System;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Data
{
	public sealed class AwsFoodRepository : IFoodRepository
	{
		private readonly IAwsClient _awsClient;
		private readonly IFoodDescriptionFactory _foodDescriptionFactory;

		public AwsFoodRepository(IAwsClient awsClient, IFoodDescriptionFactory foodDescriptionFactory)
		{
			if (awsClient == null)
				throw new ArgumentNullException("awsClient");
			if (foodDescriptionFactory == null)
				throw new ArgumentNullException("foodDescriptionFactory");

			_awsClient = awsClient;
			_foodDescriptionFactory = foodDescriptionFactory;
		}

		public async Task<FoodDescription[]> SearchByTitle(string title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");

			var ids = await _awsClient.SearchItem(title);

			var tasks = ids.Select(CreateDescription).ToArray();
			return await Task.WhenAll(tasks)
							 .ContinueWith(t => t.Result);
		}

		internal Task<FoodDescription> CreateDescription(string id)
		{
			return _foodDescriptionFactory.Create(
				id,
				_awsClient.LookupAttributes(id),
				_awsClient.LookupImages(id));
		}
	}
}