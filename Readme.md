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
        new Shop() { ShopId = 2, ShopTypeId = 3 , Name = "Skaal", Location = "S�nderborg" },
        new Shop() { ShopId = 3, ShopTypeId = 4, Name = "Lagkagehuset", Location = "Aabenraa" },
        new Shop() { ShopId = 4, ShopTypeId = 2, Name = "Ikea", Location = "Odense" },
        new Shop() { ShopId = 5, ShopTypeId = 2, Name = "Jysk", Location = "Esbjerg" }
        );
```

## ServiceLayer

Opret interface og impementering af en service i Servicelayer

### `ShopService.cs`
```C#
    public class ShopService
    {
        private readonly AppDbContext _AppDbContext = default!;

        public ShopService(AppDbContext appDbContext)
        {
            appDbContext.Database.EnsureCreated();
            _AppDbContext = appDbContext;
        }

        public IQueryable<Shop> GetShops() 
        {
            return _AppDbContext.Shops.AsNoTracking();
        }
    }
```

### `IShopService.cs`
```C#
    public interface IShopService
    {
        public IQueryable<Shop> GetShops();
    }
```

## Listview og dependency injection

### Dependency injection

Tilføj ShopService til dependency injection.
```C#
builder.Services.AddScoped<IShopService,ShopService>();
``` 

### Pages

Opret en mappe inden i pages med navn `Shops`.

Opret følgende side inden i mappen Kaldet `List`

Derefter konfigurerer følgende sider

#### `List.cshtml`
```html
<table class="table">
    @foreach (var shop in Model.Shops)
    {
        <tr>
            <td>@shop.Name</td>
            <td>@shop.Location</td>
            <td>@shop.Type.Name</td>
        </tr>
    }
</table>
```

#### `List.cshtml.cs`
```C#
    public class ListModel : PageModel
    {
        public IEnumerable<Shop> Shops { get; set; }
  
        private readonly IShopService _ShopService = default!;
    
        public ListModel(IShopService ShopService)
        {
            _ShopService = ShopService;
        }
    
    
        public void OnGet()
        {
            Shops = _ShopService.GetShops().ToList();
        }
    }
```
## Configuration af Default Route

I stedet for at lande på Index siden og skulle navigere til Shops/List siden, kan man oprette en Custom Default Route Page i `Program.cs`. Det sker ved at tilføje en RazorPagesOption til servicen, som vist her:
```C#
    builder.Services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Shops/List", "");
                });
```

Imidlertid er der en indbygget konvention som siger at siten altid skal begynde med Pages/Index og derfor bliver man nødt til at omdøbe Index pagen eller fjerne den.

