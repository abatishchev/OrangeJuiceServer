using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Builders;

namespace OrangeJuice.Server.Api.Test.Builders
{
	[TestClass]
	public class ArgumentBuilderTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AssociateTag_Is_Null()
		{
			// Arange
			const string associateTag = null;

			// Act
			Action action = () => new ArgumentBuilder(associateTag);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("associateTag");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AssociateTag_Is_Empty()
		{
			// Arange
			const string associateTag = "";

			// Act
			Action action = () => new ArgumentBuilder(associateTag);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("associateTag");
		}

		[TestMethod]
		public void BuildArgs_Should_Add_AssociateTag_And_OperationName()
		{
			// Arrange
			const string associateTag = "anyTag";
			const string operationName = "anyOperation";

			var argumentBuilder = new ArgumentBuilder(associateTag);

			// Act
			var args = argumentBuilder.BuildArgs(new Dictionary<string, string>(), operationName);

			// Assert
			args.Should().Contain(ArgumentBuilder.AssociateTagKey, associateTag)
					 .And.Contain(ArgumentBuilder.OperationNameKey, operationName);
		}
	}
}