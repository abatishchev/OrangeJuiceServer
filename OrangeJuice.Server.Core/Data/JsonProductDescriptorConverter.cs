﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public sealed class JsonProductDescriptorConverter : IConverter<string, ProductDescriptor>
	{
		public ProductDescriptor Convert(string value)
		{
			return JObject.Parse(value)
						  .ToObject<ProductDescriptor>();
		}

		public string ConvertBack(ProductDescriptor value)
		{
			return JObject.FromObject(value)
						  .ToString(Formatting.Indented);
		}
	}
}