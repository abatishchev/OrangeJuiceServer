using System.Collections.Generic;

using FluentAssertions;

using Xunit;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Web
{
	public class PercentUrlEncodingPipelineTest
	{
		[Fact]
		public void PercentEncode_Should_Encode_Special_Characters_Using_Dictionary()
		{
			// Arrange
			var charDir = new Dictionary<string, string>
			{
				{ "'", "%27" },
				{ "(", "%28" },
				{ ")", "%29" },
				{ "*", "%2A" },
				{ "!", "%21" },
				{ "~", "%7E" },
				{ "+", "%20" }
			};

			foreach (var pair in charDir)
			{
				// Act
				string encoded = PercentUrlEncodingPipeline.PercentEncode(pair.Key);

				// Assert
				encoded.Should().Be(pair.Value);
			}
		}

		[Fact]
		public void ToUpperCase_Should_UpperCase_Encoded_Characters_And_Should_Not_Regular_Characters()
		{
			// Arrange
			const string input = "word%ab%cde%25";
			const string expected = "word%AB%CDe%25";

			// Act
			string actual = PercentUrlEncodingPipeline.ToUpperCase(input);

			// Assert
			actual.Should().Be(expected);
		}
	}
}