﻿// <auto-generated />
using EfIncludeDemo.Data.Ctx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace EfIncludeDemo.Migrations
{
    [DbContext(typeof(EfCoreDemoDbContext))]
    partial class EfCoreDemoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("main")
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EfIncludeDemo.Data.Subscriptions.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Subscription");

                    b.HasDiscriminator<string>("Type").HasValue("Subscription");
                });

            modelBuilder.Entity("EfIncludeDemo.Data.User.Subscriptions.ChildSubscription", b =>
                {
                    b.Property<Guid>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime?>("ExpirationDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<Guid>("SubscriptionId");

                    b.Property<Guid>("_childId")
                        .HasColumnName("UserId");

                    b.HasKey("_id");

                    b.HasIndex("_childId");

                    b.ToTable("ChildSubscription");
                });

            modelBuilder.Entity("EfIncludeDemo.Data.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasDiscriminator<string>("Type").HasValue("User");
                });

            modelBuilder.Entity("EfIncludeDemo.Data.Subscriptions.PrivateSubscription", b =>
                {
                    b.HasBaseType("EfIncludeDemo.Data.Subscriptions.Subscription");


                    b.ToTable("PrivateSubscription");

                    b.HasDiscriminator().HasValue("211484C6-7C09-4B7F-BE98-3204016FC9C7");
                });

            modelBuilder.Entity("EfIncludeDemo.Data.User.Child", b =>
                {
                    b.HasBaseType("EfIncludeDemo.Data.User.User");


                    b.ToTable("Child");

                    b.HasDiscriminator().HasValue("E50B337D-F41C-4007-B589-7DEDB3B8377B");
                });

            modelBuilder.Entity("EfIncludeDemo.Data.User.Subscriptions.ChildSubscription", b =>
                {
                    b.HasOne("EfIncludeDemo.Data.User.Child")
                        .WithMany("_subscriptions")
                        .HasForeignKey("_childId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
