﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EfIncludeDemo.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EfIncludeDemo.Data.Ctx
{
	public abstract class Context : DbContext
	{
		private readonly StringBuilder _dropForeignKeysSqlQuery = new StringBuilder();
		private readonly StringBuilder _removeDataSqlQuery = new StringBuilder();
		protected virtual List<IEntityConfiguration> EntityConfigurations { get; } = new List<IEntityConfiguration>();
		protected virtual string Schema => null;

		protected Context()
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			EntityConfigurations.ForEach(x => x.Configure(builder));

			builder.TurnOffCascadeDelete();
			builder.HasDefaultSchema(Schema);

			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var tableName = entityType.Relational().TableName;
				if (tableName.IsEmptyString()) continue;

				_removeDataSqlQuery.AppendLine($"DELETE FROM [{Schema}].[{tableName}]");
				foreach (var fk in entityType.GetForeignKeys())
				{
					_dropForeignKeysSqlQuery.AppendLine(
						$@"IF (OBJECT_ID('{Schema}.{fk.Relational().Name}') IS NOT NULL) BEGIN " +
						$@"ALTER TABLE [{Schema}].[{tableName}] DROP CONSTRAINT {fk.Relational().Name} " +
						"END ");
				}
			}
		}

		public override DbSet<TEntity> Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public void DropForeignKeys()
		{
			var sql = _dropForeignKeysSqlQuery.ToString();
			if (!sql.IsEmptyString())
				Database.ExecuteSqlCommand(sql);
		}

		public void RemoveAllData()
		{
			var sql = _removeDataSqlQuery.ToString();
			if (!sql.IsEmptyString())
				Database.ExecuteSqlCommand(sql);
		}

		private bool ContainsType(Type type) => EntityConfigurations.Any(x => x.EntityType == type);
	}
}