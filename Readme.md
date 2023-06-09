# Model Binding

Der oprettes en mulighed for at søge (første del af navnet og case-sensitivt):

![List of shops](/Images/WebApp_List.png)

---
## ServiceLayer
ShopService udvides med en metode, der kan tage imod en søgeparameter:

<br>_`IShopService.cs`_

```C#
public interface IShopService
{
    public IQueryable<Shop> GetShops();
    public IQueryable<Shop> GetShopsByName(string? name = null); // 👈 Ny kode
}
```

<br>Her er implementationen i servicen:

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

<br>

### CSS

<br>Tilføj linket til CSS-filen fra Font Awesome i `_Layout` filen

```html
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" integrity="sha512-iecdLmaskl7CVkqkXNQ/ZH/XLlvWZOJyj7Yy7tcenmpD1ypASozpmT/E0iPtmFIB46ZmdtAc9eNBvH0H/ZpiBw==" crossorigin="anonymous" referrerpolicy="no-referrer" />
```
> 📘 Hvis du vil have en nyere version af FontAwesome kan du få den på [https://cdnjs.com/libraries/font-awesome](https://cdnjs.com/libraries/font-awesome)

<br>

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

<br> Her ses `PageModel`:

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

### ServiceLayer

<br> Tilføj `GetShopById(int)` til ShopServiceService:

```C#
    public Shop GetShopById(int ShopId)
    {
        return _AppDbContext.Shops.Find(ShopId);
    }
```
> ⚠️ Husk også at opdatere interfacet!

<br>

### WebApp

Opret en Razor Page kaldet Detail i Shops-folderen.

<br>_`Detail.cshtml`_

```html
@page
@model WebApp.Pages.Shops.DetailModel
@{
    ViewData["Title"] = "Detail";
}
 
<h2>@Model.Shop.Name</h2>
 
dl class="row">
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Shop.ShopId)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Shop.ShopId)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Shop.Location)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Shop.Location)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Shop.Type.Name)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Shop.Type.Name)
    </dd>
</dl>
 
<a asp-page="./List" class="btn btn-info">All Shops</a>
```

<br>_`Detail.cshtml.cs`_

```C#
    public class DetailModel : PageModel
    {
        public Shop Shop { get; set; }

        private readonly IShopService _ShopService = default!;

        public DetailModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        public void OnGet(int shopId)
        {
            Shop = _ShopService.GetShopById(shopId);
        }
    }   
```

<br>Tilføj ikon med link til `List` pagen:

```HTML
<table class="table">
    @foreach (var shop in Model.Shops)
    {
        <tr>
            <td>@shop.Name</td>
            <td>@shop.Location</td>
            <td>@shop.Type.Name</td>
            <!--👇 Ny kode 👇-->
            <td>
                <a asp-page="./Detail" asp-route-restaurantId="@shop.ShopId"> 
                    <i class="fas fa-info-circle"></i>                        
                </a>                                                          
            </td>
            <!--☝️ Ny kode ☝️-->                                                         
        </tr>
    }
</table>
```

Tilføjelse af Route i toppen af `Details.cs`:
```html
@page "{ShopId:int}"
```

## Håndtering af Bad Requests

<br> Hvis brugeren ændrer URL’en til et ID, der ikke findes, så kastes en exception. Dette skal undgås. Først oprettes en `NotFound.cshtml` page (uden PageModel):

```html
@page
@{
    ViewData["Title"] = "NotFound";
}
 
<h2>Your restaurant was not found</h2>
<a asp-page="./List" class="btn btn-info">See All Restaurants</a>
```

<br> Der laves en control i `OnGet()` og returntypen ændres til `IActionResult`:
```C#
    public IActionResult OnGet(int ShopId)
    {
        Shop = _ShopService.GetShopById(shopId);
    
        if (Shop == null)
        {
            return RedirectToPage("./NotFound");
        }
           
        return Page();
    }
```

![Billede af NotFound page](/Images/WebApp_NotFound.png)

