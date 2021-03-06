﻿using System.Collections.Generic;
using EfIncludeDemo.Data.Infrastructure;
using EfIncludeDemo.Data.Subscriptions;
using EfIncludeDemo.Data.User;
using EfIncludeDemo.Data.User.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace EfIncludeDemo.Data.Ctx
{
	public class EfCoreDemoDbContext : Context
	{
		protected override string Schema => "main";

		protected override List<IEntityConfiguration> EntityConfigurations => new List<IEntityConfiguration>
		{
			new SubscriptionEntityConfiguration(),
			new PrivateSubscriptionEntityConfiguration(),

			new ChildSubscriptionConfiguration(),


			new UserEntityConfiguration(),
			new ChildEntityConfiguration(),
		};

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(
				"Server=.;Database=EfIncludeDemo;user id=sa;password=sa_password;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;PersistSecurityInfo=true");
		}
	}
}