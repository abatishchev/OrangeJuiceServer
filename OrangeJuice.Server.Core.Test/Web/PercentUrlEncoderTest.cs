using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Web
{
	[TestClass]
	public class PercentUrlEncoderTest
	{
		[TestMethod]
		public void Encode_Should_Aggregate_EncodingSteps_By_Url()
		{
			// Arrange
			bool called = false;
			var encodingSteps = new Func<string, string>[]
			{
				s =>
					{
						called = true;
						return s;
					}
			};
			IUrlEncoder urlEncoder = new PercentUrlEncoder(encodingSteps);

			// Act
			urlEncoder.Encode("anyUrl");

			// Assert
			called.Should().BeTrue();
		}

		[TestMethod]
		public void PercentEncode_Should_Encode_Special_Characters_Using_Dictionary()
		{
			// Arrange
			var charDir = PercentUrlEncoder.CreateCharacterDictionary();

			foreach (var pair in charDir)
			{
				// Act
				string encoded = PercentUrlEncoder.PercentEncode(pair.Key);

				// Assert
				encoded.Should().Be(pair.Value);
			}
		}

		[TestMethod]
		public void UpperCaseEncoding_Should_UpperCase_Encoded_Characters_And_Should_Not_Regular_Characters()
		{
			// Arrange
			const string input = "word%ab%cde%25";
			const string expected = "word%AB%CDe%25";

			// Act
			string actual = PercentUrlEncoder.UpperCaseEncoding(input);

			// Assert
			actual.Should().Be(expected);
		}
	}
}