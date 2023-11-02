﻿// <auto-generated />
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Entities.Shop", b =>
                {
                    b.Property<int>("ShopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShopId"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopTypeId")
                        .HasColumnType("int");

                    b.HasKey("ShopId");

                    b.HasIndex("ShopTypeId");

                    b.ToTable("Shops");

                    b.HasData(
                        new
                        {
                            ShopId = 1,
                            Location = "Odense",
                            Name = "Power",
                            ShopTypeId = 1
                        },
                        new
                        {
                            ShopId = 2,
                            Location = "Sønderborg",
                            Name = "Skaal",
                            ShopTypeId = 3
                        },
                        new
                        {
                            ShopId = 3,
                            Location = "Aabenraa",
                            Name = "Lagkagehuset",
                            ShopTypeId = 4
                        },
                        new
                        {
                            ShopId = 4,
                            Location = "Odense",
                            Name = "Ikea",
                            ShopTypeId = 2
                        },
                        new
                        {
                            ShopId = 5,
                            Location = "Esbjerg",
                            Name = "Jysk",
                            ShopTypeId = 2
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.ShopReview", b =>
                {
                    b.Property<int>("ShopReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShopReviewId"));

                    b.Property<short>("Stars")
                        .HasColumnType("smallint");

                    b.Property<string>("subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShopReviewId");

                    b.ToTable("ShopReviews");
                });

            modelBuilder.Entity("DataLayer.Entities.ShopType", b =>
                {
                    b.Property<int>("ShopTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShopTypeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShopTypeId");

                    b.ToTable("ShopTypes");

                    b.HasData(
                        new
                        {
                            ShopTypeId = 1,
                            Name = "Electronics"
                        },
                        new
                        {
                            ShopTypeId = 2,
                            Name = "Furniture"
                        },
                        new
                        {
                            ShopTypeId = 3,
                            Name = "Restaurant"
                        },
                        new
                        {
                            ShopTypeId = 4,
                            Name = "Bakery"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Shop", b =>
                {
                    b.HasOne("DataLayer.Entities.ShopType", "Type")
                        .WithMany()
                        .HasForeignKey("ShopTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });
#pragma warning restore 612, 618
        }
    }
}