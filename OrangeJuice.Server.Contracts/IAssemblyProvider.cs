namespace OrangeJuice.Server
{
	public interface IAssemblyProvider
	{
		System.Reflection.Assembly GetExecutingAssembly();
	}
}