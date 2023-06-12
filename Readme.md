# 4.CreatingEditingData

Indeholder følgende:
* Edit og Add af Restaurant
* SELECT kontrollen til valg af Cuisine
* Validation, både server og client-side
* Post-Redirect-Get Pattern

🖼️ Her indsættes billede af Edit-viewet

## ServiceLayer

RestaurantService udvides med en `Create()`,`Edit()` metode og en `getShopTypes()` metode:

<br>`ShopService.cs`
```C#
    public Shop Update(Shop updatedShop)
    {
        _AppDbContext.Shops.Update(updatedShop);
            
        _AppDbContext.SaveChanges();

        return updatedShop;
    }  

    public Shop Add(Shop newShop)
    {
        _AppDbContext.Shops.Add(newShop);

        _AppDbContext.SaveChanges();

        return newShop;
    }

    public IQueryable<ShopType> GetShopTypes()
    {
        return _AppDbContext.ShopTypes.AsNoTracking();
    }
```
> 📘 Hvis det er, kan man lave en try/catch der fanger den exception der kommer når du laver en `SaveChanges()`.
> <br>⚠️ Husk at opdatere Interfacet.

## WebApp

Der oprettes en ny page i `Shops` med navnet `Edit`:

`Edit.cshtml.cs`
```C#
    public class EditModel : PageModel
    {
        private readonly IShopService _ShopService;

        public int ShopId { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; } = default!;

        [BindProperty]
        public Shop Shop { get; set; } = default!;

        public EditModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        public IActionResult OnGet()
        {
            Shop = _ShopService.GetShopById(ShopId);

            if(Shop == null)
            {
                return RedirectToPage("./NotFound");
            }

            Types = _ShopService.GetShopTypes()
                                .Select(t => new SelectListItem(t.Name,t.ShopTypeId.ToString()))
                                .ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                _ShopService.Update(Shop);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Page();
        }
    }
```

<br>`Edit.cshtml`
```html
@page "{ShopId:int}"
@model WebApp.Pages.Shops.EditModel

<form action="Post">

    <div class="form-group">
        <label asp-for="Shop.Name" />
        <input  type="text" asp-for="Shop.Name" class="form-control"/>
    </div>

    <div class="form-group">
        <label asp-for="Shop.Location" />
        <input type="text" asp-for="Shop.Location" class="form-control"/>
    </div>

    <div class="form-group">
        <label asp-for="Shop.Type" />
        <select asp-for="Shop.Type"
                asp-items="Model.Types"
                class="form-control">
            <option value="" disabled>Choose ShopType</option>
        </select>
    </div>

    <button><i class="fa-solid fa-floppy-disk"></i> Save</button>

</form>
```



