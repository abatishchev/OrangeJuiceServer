﻿using System.Threading.Tasks;

namespace OrangeJuice.Server.Configuration
{
	public interface IOptionsProvider<T>
	{
		Task<T[]> GetOptions();
	}
}