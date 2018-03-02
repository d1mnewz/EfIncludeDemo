using System;
using System.Linq;
using System.Threading.Tasks;
using EfIncludeDemo.Data.Ctx;
using EfIncludeDemo.Data.Subscriptions;
using EfIncludeDemo.Data.User;
using Microsoft.EntityFrameworkCore;

namespace EfIncludeDemo
{
	public static class Program
	{
		static async Task Main()
		{
			var builder = new DbContextOptionsBuilder<EfCoreDemoDbContext>();
			builder.UseSqlServer(
				"Server=.;Database=EfIncludeDemo;user id=sa;password=sa_password;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;PersistSecurityInfo=true");
			using (var context = new EfCoreDemoDbContext(builder.Options))
			{
				await context.Database.MigrateAsync();
				context.Add(new Child("hello@world.com"));
				context.Add(new PrivateSubscription("sub"));
				await context.SaveChangesAsync();
				var children = context.Set<Child>().ToList();
				var privateSubscriptions = context.Set<PrivateSubscription>().ToList();
				children.First().WithPrivateSubscription(privateSubscriptions.First(), DateTime.Now);
				await context.SaveChangesAsync();
			}

			Console.WriteLine("Hello World!");
		}
	}
}