using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public static bool TestMode = false;

        public DbSet<Shop> Shops { get; set; } = default!;
        public DbSet<ShopType> ShopTypes { get; set; } = default!;
        public DbSet<ShopReview> ShopReviews { get; set; } = default!;

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if(!TestMode)
            {
                modelBuilder.Entity<ShopType>().HasData(
                    new ShopType { ShopTypeId = 1, Name = "Electronics" },
                    new ShopType { ShopTypeId = 2, Name = "Furniture" },
                    new ShopType { ShopTypeId = 3, Name = "Restaurant" },
                    new ShopType { ShopTypeId = 4, Name = "Bakery"}
                );

                modelBuilder.Entity<Shop>().HasData(
                    new Shop() { ShopId = 1, ShopTypeId = 1 , Name = "Power", Location = "Odense" },
                    new Shop() { ShopId = 2, ShopTypeId = 3 , Name = "Skaal", Location = "Sønderborg" },
                    new Shop() { ShopId = 3, ShopTypeId = 4, Name = "Lagkagehuset", Location = "Aabenraa" },
                    new Shop() { ShopId = 4, ShopTypeId = 2, Name = "Ikea", Location = "Odense" },
                    new Shop() { ShopId = 5, ShopTypeId = 2, Name = "Jysk", Location = "Esbjerg" }
                );
            }
        }
    }
}
