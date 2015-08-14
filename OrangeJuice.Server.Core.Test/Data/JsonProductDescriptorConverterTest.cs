using System;

using FluentAssertions;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

using Xunit;

namespace OrangeJuice.Server.Test.Data
{
	public class JsonProductDescriptorConverterTest
	{
		[Theory]
		[InlineData(typeof(JsonProductDescriptorConverter))]
		[InlineData(typeof(FSharp.Data.JsonProductDescriptorConverter))]
		public void Convert_And_ConverterBack_Should_Convert_ProductDescriptor_To_String_And_Back(Type type)
		{
			// Arrange
			var expected = new ProductDescriptor
			{
				ProductId = Guid.Parse("8f76a6f5-037f-41b4-9837-de9b5c5ff926"),
				SourceProductId = "A00BCD123EF",
				Title = "Title",
				Brand = "Brand",
				SmallImageUrl = "http://example.com/smallImageUrl",
				MediumImageUrl = "http://example.com/mediumImageUrl",
				LargeImageUrl = "http://example.com/largeImageUrl",
			};

			var converter = CreateConverter(type);

			// Act
			string json = converter.ConvertBack(expected);
			ProductDescriptor actual = converter.Convert(json);

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}

		private static IConverter<string, ProductDescriptor> CreateConverter(Type type)
		{
			return (IConverter<string, ProductDescriptor>)Activator.CreateInstance(type);
		}
	}
}