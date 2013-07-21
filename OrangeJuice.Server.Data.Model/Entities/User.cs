using System;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Data.Model
// ReSharper restore CheckNamespace
{
	public partial class User
	{
		private User()
		{
		}

		internal static User CreateNew()
		{
			return new User
			{
				UserGuid = Guid.NewGuid()
			};
		}
	}
}