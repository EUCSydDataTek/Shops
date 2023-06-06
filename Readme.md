# List of Shops

## Oversigt:

* Opretelse af Entities
* Data Seeding
* Service interface, class and methods
* ListView og Dependency Injection
* Lave en Custom Default Route

## Oprettelse af Entities

* I DataLayer opret en mappe med navn `Entities`.
* Opret følgende klasser med indhold i mappen.

### ShopType.cs
```C#
    public class ShopType
    {
        public int ShopTypeId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
```

### ShopReview.cs
```C#
    public class ShopReview
    {
        public int ShopReviewId { get; set; }

        public string subject { get; set; } = default!;

        public string text { get; set; } = default!;

        public short Stars { get; set; }
    }
```

### Shop.cs
```C#
    public class Shop
    {
        public int ShopId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ShopTypeId { get; set; }

        public string Location { get; set; } = string.Empty;

        public ShopType Type { get; set; } = default!;

    }
```

## Data Seeding

### AppDbContext.cs

Indsæt disse properties til `AppDbContext`.

```C#
    public DbSet<Shop> Shops { get; set; }
    public DbSet<ShopType> ShopTypes { get; set; }
    public DbSet<ShopReview> ShopReviews { get; set; }
```

Indsæt derefter kode i `OnModelCreating` metoden.
```C#
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
```

