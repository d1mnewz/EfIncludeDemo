using System;
using System.Collections.Generic;
using System.Linq;
using EfIncludeDemo.Data.Infrastructure;
using EfIncludeDemo.Data.Subscriptions;
using EfIncludeDemo.Data.User.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.User
{
	[Serializable]
	public class Child : User
	{
		private Child()
		{
		}

		public Child(string email) : base(email)
		{
		}

		private ICollection<ChildSubscription> _subscriptions { get; set; } = new HashSet<ChildSubscription>();


		public IEnumerable<ChildSubscription> Subscriptions => _subscriptions.OrderByDescending(x => x.ExpirationDate).AsEnumerable();


		public void WithPrivateSubscription(PrivateSubscription subscription, DateTime startDate)
		{
			// verify that subscriptions are in valid state
			// TODO: Include?
			_subscriptions.Add(new ChildSubscription(subscription, startDate));
			// TODO: add event
		}
	}

	public class ChildEntityConfiguration : EntityDiscriminatorValueConfiguration<Child>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<Child> cfg)
		{
			cfg.Ignore(x => x.Subscriptions);

			cfg.HasMany(typeof(ChildSubscription), "_subscriptions")
				.WithOne()
				.HasForeignKey("_childId");

			base.Initialize(builder, cfg);
		}
	}
}