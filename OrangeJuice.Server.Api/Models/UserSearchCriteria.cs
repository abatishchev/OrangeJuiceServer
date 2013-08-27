using System;

namespace OrangeJuice.Server.Api.Models
{
	/// <seealso cref="OrangeJuice.Server.Api.Validation.UserSearchCriteriaValidator" />
	public class UserSearchCriteria
	{
		public Guid? UserGuid { get; set; }
	}
}