using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.Infrastructure
{
	public interface IEntityConfiguration
	{
		Type EntityType { get; }
		void Configure(ModelBuilder builder);
	}

	public abstract class EntityConfiguration : IEntityConfiguration
	{
		public abstract Type EntityType { get; }
		public abstract void Configure(ModelBuilder builder);
	}

	public abstract class EntityConfiguration<T> : EntityConfiguration where T : class
	{
		public override Type EntityType => typeof(T);
		protected virtual string TableName => EntityType.Name;

		public override void Configure(ModelBuilder builder)
		{
			var cfg = builder.Entity<T>();
			cfg.ToTable(TableName);

			Initialize(builder, cfg);
		}


		protected virtual void Initialize(ModelBuilder builder, EntityTypeBuilder<T> cfg)
		{
		}
	}

	public abstract class EntityDiscriminatorValueConfiguration<T> : EntityConfiguration where T : class
	{
		public override Type EntityType => typeof(T);

		public override void Configure(ModelBuilder builder)
		{
			var cfg = builder.Entity<T>();

			Initialize(builder, cfg);
		}

		protected virtual void Initialize(ModelBuilder builder, EntityTypeBuilder<T> cfg)
		{
		}
	}
}