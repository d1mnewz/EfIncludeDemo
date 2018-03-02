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
			var context = new EfCoreDemoDbContext();

			await context.Database.MigrateAsync();
			context.Add(new Child("hello@world.com"));
			context.Add(new PrivateSubscription("sub"));
			await context.SaveChangesAsync();

			context.Dispose();
			context = new EfCoreDemoDbContext();

			var child = context.Set<Child>().First();
			var privateSubscription = context.Set<PrivateSubscription>().First();
			child.WithPrivateSubscription(privateSubscription, DateTime.Now);
			context.Update(child);
			var entries = context.ChangeTracker.Entries();
			await context.SaveChangesAsync();

			Console.WriteLine("Hello World!");
		}
	}
}