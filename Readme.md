# Searching and Paging
Denne branch viser Search kombineret med Paging.
> 🔗 Ref: [Simple Paging In ASP.NET Core Razor Pages](https://www.mikesdotnetting.com/article/328/simple-paging-in-asp-net-core-razor-pages)
---
## ServiceLayer
Der oprettes en mappe i roden kaldet __Models__.

Der oprettes en ViewModel i __Models__-folderen:
```C#
    using DataLayer.Entities;

    public class ShopViewModel
    {
        public List<Shop> Shops { get; set; } = default!;
        public int TotalCount { get; set; }
    }
```

<br>ShopServicen udbygges med denne metode:
```C#
    public ShopViewModel GetShopsByName(string searchTerm, int currentPage, int pageSize)
    {
        ShopViewModel ShopModel = new();
        var query = _AppDbContext.Shops.Include(s => s.Type).AsNoTracking();
        query = searchTerm != null ? query.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower())).OrderBy(r => r.Name) : query;

        ShopModel.TotalCount = query.Count();

        ShopModel.Shops = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        return ShopModel;
    }
```
> ⚠️ Husk at opdatere interfacet
---
## WebApp

<br>PageModel i `Shops/List.cshtml.cs` udvides med følgende properties og en Enum:
```C#
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public int Count { get; set; }

    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

    public enum PageSizeEnum
    {
        [Display(Name = "2")]
        _2 = 2,
        [Display(Name = "4")]
        _4 = 4,
        [Display(Name = "10")]
        _10 = 10,
    }
```

<br>I `OnGet()` tilføjes følgende:
```C#
    public void OnGet()
    {
        ShopViewModel ShopModel = _ShopService.GetShopsByName(SearchTerm, CurrentPage, PageSize);
        Shops = ShopModel.Shops;
        Count = ShopModel.TotalCount;
    }
```

<br> I Razor pagen tilføjes følgende:
```html
    <tbody>
        @foreach (var Shop in Model.Shops)
        {
            <tr>
                <td>@Shop.Name</td>
                <td>@Shop.Location</td>
                <td>@Shop.Type.Name</td>
                <td>
                        <a asp-page="./Detail" asp-route-restaurantId="@Shop.ShopId">
                            <i class="fas fa-info-circle"></i>
                        </a>
                    </td>
            </tr>
        }
    </tbody>
```

<br> Udskift søgeformen så den understøtter paging:
```html
    <form method="get">
        <div class="input-group mb-3">
            <input type="search" asp-for="@Model.SearchTerm" class="form-control">
            <div class="input-group-append">
                <select asp-for="@Model.PageSize"
                        asp-items="Html.GetEnumSelectList<WebApp.Pages.Shops.ListModel.PageSizeEnum>()"
                        class="custom-select">
                    <option value="">Pagesize</option>
                </select>
                <button class="btn btn-outline-secondary" type="submit">
                    <i class="fas fa-search"></i>
                </button>
            </div>
        </div>
    </form>
```

<br>Placer paging i bunden af siden:
```C#
    <div>
        <ul class="pagination">
            @for (var i = 1; i <= Model.TotalPages; i++)
            {
                <li class="page-item @(i == Model.CurrentPage ? "active" : "")">
                    <a asp-page=""
                    asp-route-currentpage="@i" 
                    asp-route-searchTerm="@Model.SearchTerm" 
                    asp-route-pageSize="@Model.PageSize"
                    class="page-link">@i</a>
                </li>
            }
        </ul>
    </div>
```

