using System;
using EfIncludeDemo.Data.Infrastructure;
using EfIncludeDemo.Data.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.User.Subscriptions
{
	[Serializable]
	public class ChildSubscription
	{
		private ChildSubscription()
		{
			_id = Guid.NewGuid();
		}

		public ChildSubscription(PrivateSubscription subscription, DateTime startDate) : this()
		{
			SubscriptionId = subscription.Id;
			StartDate = startDate;
		}

		private Guid _id { get; set; }
		private Guid _childId { get; set; }
		public Guid SubscriptionId { get; private set; }

		public DateTime StartDate { get; private set; }
		public DateTime? ExpirationDate { get; private set; }

		public void ExpiresAt(DateTime expirationDate)
		{
			ExpirationDate = expirationDate;
		}
	}

	public class ChildSubscriptionConfiguration : EntityConfiguration<ChildSubscription>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<ChildSubscription> cfg)
		{
			cfg.Property("_id").HasColumnName("Id");
			cfg.HasKey("_id");
			cfg.Property("_childId").HasColumnName("UserId");


			base.Initialize(builder, cfg);
		}
	}
}