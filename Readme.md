# Model Binding

Der oprettes en mulighed for at søge (første del af navnet og case-sensitivt):


---
## ServiceLayer
ShopService udvides med en metode, der kan tage imod en søgeparameter:

_IShopService.cs_
```C#
public interface IShopService
{
    public IQueryable<Shop> GetShops();
    public IQueryable<Shop> GetShopsByName(string? name = null); // 👈 Ny kode
}
```
Her er implementationen i servicen:

_ShopService.cs_
```C#
    public IQueryable<Shop> GetShopsByName(string? name = null) 
    {
        return _AppDbContext.Shops
                                .Include(s => s.Type)
                                .Where(s => string.IsNullOrEmpty(name) || s.Name.StartsWith(name))
                                .AsNoTracking();
    }
```
---
## WebApp

### CSS
tilføj linket til CSS-filen fra Font Awesome i `_Layout` filen
```html
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" integrity="sha512-iecdLmaskl7CVkqkXNQ/ZH/XLlvWZOJyj7Yy7tcenmpD1ypASozpmT/E0iPtmFIB46ZmdtAc9eNBvH0H/ZpiBw==" crossorigin="anonymous" referrerpolicy="no-referrer" />
```
> 📘 Hvis du vil have en nyere version af FontAwesome kan du få den på [https://cdnjs.com/libraries/font-awesome](https://cdnjs.com/libraries/font-awesome)

### Html
Tilføj en searchbox til List view. Med BootStrap laves en searchbox således:

```html
<form method="get">
    <div class="input-group mb-3">
        <input type="search" asp-for="SearchTerm" class="form-control">
        <div class="input-group-append">
            <button class="btn btn-outline-secondary" type="submit">
                <i class="fas fa-search"></i>
            </button>
        </div>
    </div>
</form>
```

> 📘 Man kan evt. demonstrere en input uden brug af `asp-for`, der så vil kræve både `id`, `name` og `value`:
> ```html
> <input type="search" id="SearchTerm" name="SearchTerm" value="@Model.SearchTerm">
> ```
> <br>
---
Her ses `PageModel`:
```C#
    public class ListModel : PageModel
    {
        public IEnumerable<Shop> Shops { get; set; }

        [BindProperty(SupportsGet = true)] // 👈 Ny
        public string SearchTerm { get; set; } // 👈 Ny
  
        private readonly IShopService _ShopService = default!;
    
        public ListModel(IShopService ShopService)
        {
            _ShopService = ShopService;
        }
    
        public void OnGet()
        {
            Shops = _ShopService.GetShopsByName(SearchTerm).ToList(); // 👈 Ændret
        }
    }
```
Bemærk at der laves en TwoWay Databinding ved at tilføje `[BindProperty]` til SearchTerm. Og at den bringes til at supportere `GET`.

> 📘 Man man også vise to andre (og mindre smarte) måder: Uden Model Binding:
> ```C#
> string searchTerm = HttpContext.Request.QueryString.Value.Split('=').LastOrDefault();
> ```
> 
> One-Way binding via parameter:
> ```C#
> public void OnGet(string searchTerm)
> {
>    Restaurants = _restaurantService.GetRestaurantsByName(searchTerm).ToList();
> }
> ```
> <br>
---
## Detail page

