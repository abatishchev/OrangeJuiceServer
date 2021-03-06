﻿using System;

using FluentValidation.Attributes;

namespace OrangeJuice.Server.Data.Models
{
	[Validator(typeof(Validation.RatingModelValidator))]
	public class RatingModel
	{
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }

		public byte Value { get; set; }

		public string Comment { get; set; }
	}
}