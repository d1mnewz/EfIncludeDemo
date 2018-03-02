using System;
using EfIncludeDemo.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.User
{
	[Serializable]
	public abstract class User : AggregateRoot
	{
		protected User()
		{
		}

		protected User(string email) : this()
		{
			Email = email;
		}

		public string Email { get; protected set; }
	}

	public class UserEntityConfiguration : EntityConfiguration<User>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<User> cfg)
		{
			cfg.Property(x => x.Email).NVarChar(512);


			cfg.HasDiscriminator<string>("Type")
				.HasValue<Child>("E50B337D-F41C-4007-B589-7DEDB3B8377B");
			base.Initialize(builder, cfg);
		}
	}
}