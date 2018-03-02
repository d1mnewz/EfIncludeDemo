using System;
using EfIncludeDemo.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.Subscriptions
{
	[Serializable]
	public class PrivateSubscription : Subscription
	{
		private PrivateSubscription()
		{
		}

		public PrivateSubscription(string name) : base(name)
		{
		}
	}

	public class PrivateSubscriptionEntityConfiguration : EntityDiscriminatorValueConfiguration<PrivateSubscription>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<PrivateSubscription> cfg)
		{
			base.Initialize(builder, cfg);
		}
	}
}