using System;
using System.Linq.Expressions;

namespace OrangeJuice.Server.Web
{
	public interface IUrlProvider
	{
		Uri UriFor<TController>(Expression<Action<TController>> action);
	}
}