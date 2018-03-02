using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfIncludeDemo.Data.Ctx
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EfCoreDemoDbContext>
	{
		public EfCoreDemoDbContext CreateDbContext(string[] args)
		{
			var builder = new DbContextOptionsBuilder<EfCoreDemoDbContext>();
			var connectionString =
				"Server=.;Database=EfIncludeDemo;user id=sa;password=sa_password;Encrypt=True;TrustServerCertificate=True;Connection Timeout=60;PersistSecurityInfo=true";
			builder.UseSqlServer(connectionString);
			return new EfCoreDemoDbContext(builder.Options);
		}
	}
}