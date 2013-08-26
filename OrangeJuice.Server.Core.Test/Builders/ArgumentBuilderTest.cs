﻿using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Builders;

namespace OrangeJuice.Server.Test.Builders
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
		public void BuildArgs_Should_Add_Arguments()
		{
			// Arrange
			const string associateTag = "anyTag";

			var argumentBuilder = new ArgumentBuilder(associateTag);

			// Act
			var args = argumentBuilder.BuildArgs(new Dictionary<string, string>());

			// Assert
			args.Should().Contain(ArgumentBuilder.AssociateTagKey, associateTag)
						 .And.Contain(ArgumentBuilder.ServiceKey, ArgumentBuilder.ServiceValue)
						 .And.Contain(ArgumentBuilder.ConditionKey, ArgumentBuilder.ConditionValue);
		}
	}
}