namespace OrangeJuice.Server.Data
{
	public interface IProductDescriptorFactory<in T>
	{
		ProductDescriptor Create(T item);
	}
}