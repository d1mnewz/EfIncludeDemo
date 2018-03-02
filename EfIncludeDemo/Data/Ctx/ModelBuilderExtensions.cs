using System.Linq;
using EfIncludeDemo.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EfIncludeDemo.Data.Ctx
{
	public static class ModelBuilderExtensions
	{
		public static void TurnOffCascadeDelete(this ModelBuilder mb)
		{
			foreach (var relationship in mb.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				var type = ((TypeBase) relationship.DeclaringEntityType).ClrType;
				if (type.ImplementsInterfaceTemplate(typeof(IValueObject<>)))
					continue;

				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}

		public static void UseClassNameAsTableName(this ModelBuilder mb)
		{
			foreach (var entity in mb.Model.GetEntityTypes())
			{
				entity.Relational().TableName = entity.DisplayName();
			}
		}
	}
}