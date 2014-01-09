using System;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Data
{
	public partial class Rating : IRating
	{
		public Guid UserGuid
		{
			get
			{
				return User != null ?
					User.UserGuid :
					Guid.Empty;
			}
		}
	}
}