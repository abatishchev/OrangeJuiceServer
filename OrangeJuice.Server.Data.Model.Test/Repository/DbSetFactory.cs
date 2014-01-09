using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Moq;

namespace OrangeJuice.Server.Data.Model.Test.Repository
{
	internal static class DbSetFactory
	{
		public static DbSet<T> Create<T>(IEnumerable<T> data) where T : class
		{
			var async = new TestDbAsyncEnumerable<T>(data);
			var query = async.AsQueryable();

			var setMock = new Mock<DbSet<T>>();

			setMock.As<IQueryable<T>>().Setup(s => s.Provider).Returns(async.Provider);
			setMock.As<IQueryable<T>>().Setup(s => s.Expression).Returns(query.Expression);
			setMock.As<IQueryable<T>>().Setup(s => s.ElementType).Returns(query.ElementType);
			setMock.As<IQueryable<T>>().Setup(s => s.GetEnumerator()).Returns(query.GetEnumerator());

			return setMock.Object;
		}

		internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>
		{
			public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
				: base(enumerable)
			{ }

			public TestDbAsyncEnumerable(Expression expression)
				: base(expression)
			{ }

			public IDbAsyncEnumerator GetAsyncEnumerator()
			{
				return ((IDbAsyncEnumerable<T>)this).GetAsyncEnumerator();
			}

			IDbAsyncEnumerator<T> IDbAsyncEnumerable<T>.GetAsyncEnumerator()
			{
				return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
			}

			public IQueryProvider Provider
			{
				get { return new TestDbAsyncQueryProvider<T>(this); }
			}
		}

		internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
		{
			private readonly IQueryProvider _inner;

			internal TestDbAsyncQueryProvider(IQueryProvider inner)
			{
				_inner = inner;
			}

			public IQueryable CreateQuery(Expression expression)
			{
				return new TestDbAsyncEnumerable<TEntity>(expression);
			}

			public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
			{
				return new TestDbAsyncEnumerable<TElement>(expression);
			}

			public object Execute(Expression expression)
			{
				return _inner.Execute(expression);
			}

			public TResult Execute<TResult>(Expression expression)
			{
				return _inner.Execute<TResult>(expression);
			}

			public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
			{
				return Task.FromResult(Execute(expression));
			}

			public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
			{
				return Task.FromResult(Execute<TResult>(expression));
			}
		}

		internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
		{
			private readonly IEnumerator<T> _inner;

			public TestDbAsyncEnumerator(IEnumerator<T> inner)
			{
				_inner = inner;
			}

			public void Dispose()
			{
				_inner.Dispose();
			}

			public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
			{
				return Task.FromResult(_inner.MoveNext());
			}

			public T Current
			{
				get { return _inner.Current; }
			}

			object IDbAsyncEnumerator.Current
			{
				get { return Current; }
			}
		}
	}
}