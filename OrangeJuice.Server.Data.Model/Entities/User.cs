using System;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Data.Model
{
	public partial class User : IUser
	{
		#region Constructors
		private User()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		internal static User CreateNew(string email = null)
		{
			return new User
			{
				UserGuid = Guid.NewGuid(),
				Email = email
			};
		}
		#endregion
	}
}