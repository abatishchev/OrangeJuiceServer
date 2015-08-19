using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Data.Models
{
	public class Product
	{
		public Product()
		{
			this.Ratings = new HashSet<Rating>();
		}

		public Guid ProductId { get; set; }

		public string Barcode { get; set; }

		public Ab.Amazon.Data.BarcodeType BarcodeType { get; set; }

		public string SourceProductId { get; set; }

		public virtual ICollection<Rating> Ratings { get; set; }
	}
}