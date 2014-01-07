using System;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Data.Model
{
	internal partial class Rating : IRating
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