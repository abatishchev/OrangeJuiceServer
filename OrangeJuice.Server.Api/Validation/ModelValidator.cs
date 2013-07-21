using System;
using System.Web.Http.ModelBinding;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class ModelValidator : IModelValidator
	{
		#region Fields
		private static IModelValidator _current;

		private static readonly Lazy<IModelValidator> _default = new Lazy<IModelValidator>(CreateDefault);
		#endregion

		#region Properties
		public static IModelValidator Current
		{
			get { return _current ?? (_current = _default.Value); }
			set { _current = value; }
		}
		#endregion

		#region Methods
		private static IModelValidator CreateDefault()
		{
			return new ModelValidator();
		}

		public bool IsValid(ModelStateDictionary modelState)
		{
			return modelState.IsValid;
		}
		#endregion
	}
}