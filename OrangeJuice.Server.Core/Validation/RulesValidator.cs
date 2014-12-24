using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server.Validation
{
	public abstract class RulesValidator<T> : IValidator<T>
	{
		public bool IsValid(T item)
		{
			return GetRules(item).All(b => b);
		}

		protected abstract IEnumerable<bool> GetRules(T item);
	}

	public abstract class RulesValidator<T, TResult> : RulesValidator<T>, IValidator<T, TResult>
	{
		public TResult ValidationResult { get; protected set; }
	}
}