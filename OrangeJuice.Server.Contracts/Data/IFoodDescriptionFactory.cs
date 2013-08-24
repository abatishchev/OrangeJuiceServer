namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		FoodDescription Create(System.Xml.Linq.XElement element);
	}
}