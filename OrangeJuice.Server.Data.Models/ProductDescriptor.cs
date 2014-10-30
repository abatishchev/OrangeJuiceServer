﻿using System;

namespace OrangeJuice.Server.Data.Models
{
	public class ProductDescriptor
	{
		public Guid ProductId { get; set; }
		
		public string SourceProductId { get; set; }

		public string Title { get; set; }

		public string Brand { get; set; }

		public string SmallImageUrl { get; set; }

		public string MediumImageUrl { get; set; }

		public string LargeImageUrl { get; set; }
	}
}