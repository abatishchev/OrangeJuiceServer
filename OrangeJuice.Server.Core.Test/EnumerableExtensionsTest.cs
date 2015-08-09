using System.Linq;

using FluentAssertions;

using Xunit;

namespace OrangeJuice.Server.Test
{
	public class EnumerableExtensionsTest
	{
		[Fact]
		public void Except_Should_Return_Sequence_Items()
		{
			// Arrange
			var seq = new[] { 1, 2, 3 };

			// Act
			var actual = seq.Except(2);

			// Assert
			actual.ShouldAllBeEquivalentTo(new[] { 1, 3 });
		}

		[Fact]
		public void AsInfinite_Should_Return_Repeating_Sequence()
		{
			// Arrange
			const int number = 3;

			var data = Enumerable.Range(1, number).ToArray();
			var expected = Enumerable.Repeat(data, number).SelectMany(seq => seq).ToArray();

			// Act
			var actual = data.AsInfinite().Take(data.Length * number).ToArray();

			// Assert
			actual.ShouldAllBeEquivalentTo(expected);
		}
	}
}