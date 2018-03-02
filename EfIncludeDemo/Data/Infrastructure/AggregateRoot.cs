using System;

namespace EfIncludeDemo.Data.Infrastructure
{
	public interface IAggregateRoot : IEntity
	{
	}

	public interface IAggregateRoot<out TId> : IAggregateRoot, IEntity<TId>
	{
	}

	[Serializable]
	public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
	{
		protected AggregateRoot()
		{
		}

		protected AggregateRoot(TId id) : base(id)
		{
		}

	}

	[Serializable]
	public abstract class AggregateRoot : AggregateRoot<Guid>
	{
		protected AggregateRoot() : base(Guid.NewGuid())
		{
		}

		protected AggregateRoot(Guid id) : base(id)
		{
		}
	}
}