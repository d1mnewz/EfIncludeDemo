using System;
using EfIncludeDemo.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.Subscriptions
{
	[Serializable]
	public abstract class Subscription : AggregateRoot
	{
		protected Subscription()
		{
		}

		protected Subscription(string name) : this()
		{
			Name = name;
		}

		public string Name { get; private set; }
	}

	public class SubscriptionEntityConfiguration : EntityConfiguration<Subscription>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<Subscription> cfg)
		{
			cfg.HasDiscriminator<string>("Type")
				.HasValue<PrivateSubscription>("211484C6-7C09-4B7F-BE98-3204016FC9C7");


			base.Initialize(builder, cfg);
		}
	}
}