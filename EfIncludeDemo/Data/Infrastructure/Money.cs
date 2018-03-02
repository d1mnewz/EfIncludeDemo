using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfIncludeDemo.Data.Infrastructure
{
	[Serializable]
	public class Money : ValueObject<Money>
	{
		private Money()
		{
		}

		private Money(decimal value, string currency)
		{
			Value = value;
			Currency = currency.ToUpper();
		}

		public decimal Value { get; private set; }

		/// <summary>
		/// <a href="http://currencysystem.com/codes/">ISO 4217</a> currency code 
		/// </summary>
		public string Currency { get; private set; }

		public static Money Dkk(decimal value) => new Money(value, "DKK");
		public static Money Usd(decimal value) => new Money(value, "USD");
	}

	public class MoneyConfiguration
	{
		public static void Configure<TOwner>(ReferenceOwnershipBuilder<TOwner, Money> builder) where TOwner : class
		{
			builder.Property(x => x.Value);
			builder.Property(x => x.Currency).NVarChar(3);
		}
	}

	public static class EntityTypeConfigurationExtensions
	{
		public static PropertyBuilder NVarCharMax(this PropertyBuilder cfg) => cfg.NVarChar();

		public static PropertyBuilder NVarChar(this PropertyBuilder cfg, int? maxLength = null)
		{
			cfg = maxLength.HasValue ? cfg.HasMaxLength(maxLength.Value) : cfg;
			return cfg.Required();
		}

		public static PropertyBuilder Optional(this PropertyBuilder cfg) => cfg.IsRequired(false);

		public static PropertyBuilder Required(this PropertyBuilder cfg) => cfg.IsRequired(true);
	}
}