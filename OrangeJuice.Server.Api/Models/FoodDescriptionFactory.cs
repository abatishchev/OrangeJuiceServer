using System;
using System.Xml.Linq;

namespace OrangeJuice.Server.Api.Models
{
	public sealed class FoodDescriptionFactory : IFoodDescriptionFactory
	{
		public FoodDescription Create(XElement element)
		{
			throw new NotImplementedException();
		}
	}
}