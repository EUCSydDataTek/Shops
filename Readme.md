# 4.CRUD Data

Indeholder følgende:
* CRUD operationer
    * C = Create
    * R = Read (er gjordt i et tidligere eksempel) 
    * U = Update
    * D = Delete
* Validation, både server og client-side
* Post-Redirect-Get Pattern

![](/Images/WebApp_Add.png)

# ServiceLayer

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

    public bool Delete(int ShopId)
    {
        var shop = _AppDbContext.Shops.Where(s => s.ShopId == ShopId).FirstOrDefault();

        if (shop != null)
        {
            return false;
        }

        _AppDbContext.Shops.Remove(shop);

        try
        {
            _AppDbContext.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public IQueryable<ShopType> GetShopTypes()
    {
        return _AppDbContext.ShopTypes.AsNoTracking();
    }
```
> 📘 Hvis det er, kan man lave en try/catch der fanger den exception der kommer når du laver en `SaveChanges()`.
> <br>⚠️ Husk at opdatere Interfacet.

# WebApp

## Create

Der oprettes nu en ny page i `Shops` med navnet `Create`:

'Create.cshtml'
```html
    @page
    @model WebApp.Pages.Shops.CreateModel

    <form method="post">

        <div class="form-group">
            <label asp-for="Shop.Name" />
            <input type="text" asp-for="Shop.Name" class="form-control" />
        </div>
        <br>
        <div class="form-group">
            <label asp-for="Shop.Location" />
            <input type="text" asp-for="Shop.Location" class="form-control" />
        </div>
        <br>
        <div class="form-group">
            <label asp-for="Shop.Type.ShopTypeId" />
            <select asp-for="Shop.ShopTypeId"
                    asp-items="Model.Types"
                    class="form-control">
                <option value="" disabled>Choose ShopType</option>
            </select>
        </div>
        <br>

        <button><i class="fa-solid fa-plus"></i> Add</button>

    </form>
```

Der tilføjes nu kode i page behind som opretter en shop:

'Create.cshtml.cs'
```C#
    public class CreateModel : PageModel
    {
        private readonly IShopService _ShopService;

        public CreateModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        [BindProperty]
        public Shop Shop { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; } = default!;

        public void OnGet()
        {
            Types = _ShopService.GetShopTypes()
                    .Select(t => new SelectListItem(t.Name, t.ShopTypeId.ToString()))
                    .ToList();
        }

        public IActionResult OnPost()
        {
            var newShop = _ShopService.Add(Shop);

            return RedirectToPage("/Shops/Detail", new { ShopId = Shop.ShopId });
        }
    }
```

## Update

Der oprettes nu en ny page i `Shops` med navnet `Edit`:

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

<form method="post">

    <input type="hidden" asp-for="Shop.ShopId" value="@Model.Shop.ShopId">

    <div class="form-group">
        <label asp-for="Shop.Name" />
        <input  type="text" asp-for="Shop.Name" class="form-control"/>
    </div>
    <br>

    <div class="form-group">
        <label asp-for="Shop.Location" />
        <input type="text" asp-for="Shop.Location" class="form-control"/>
    </div>
    <br>

    <div class="form-group">
        <label asp-for="Shop.Type.ShopTypeId" />
        <select asp-for="Shop.ShopTypeId"
                asp-items="Model.Types"
                class="form-control">
            <option value="" disabled>Choose ShopType</option>
        </select>
    </div>
    <br>

    <button><i class="fa-solid fa-floppy-disk"></i> Save</button>

</form>
```

## Delete

At fjerne et element kan gøres på to måder:
* __Soft delete__ 
    * Du Sletter ikke data fra databasen men du markerer med at den ikke er synlig på forsiden ved at have en boolsk værdi som fortæller det.
* __Hard delete__ 
    * Sletter den permanent fra databasen.

> I dette eksempel bruger vi __hard delete__ med en promt der spøger dig om du er sikker på at du vil slette for at ungå at vi gør det ved et uheld.

<br>![Billede af promt](/Images/Delete_Promt.png)

<br> Der oprettes nu en ny page i `Shops` med navnet `Delete`:

`Delete.cshtml`
```html
    @page
    @model DeleteModel

    <h1>Are you Sure?</h1>

    <h3>Delete @Model.Shop.Name?</h3>

    <div class="alert alert-danger">This action cannot be reverted!</div>

    <a class="btn btn-success p-2 w-25" asp-page-handler="DeleteShop" asp-route-ShopId="@Model.Shop.ShopId">Yes</a>
    <a class="btn btn-danger p-2 w-25" asp-page="/Shops/List">No</a>
```

`Delete.cshtml.cs`
```C#
    public class DeleteModel : PageModel
    {
        private readonly IShopService _ShopService;

        [BindProperty(SupportsGet = true)]
        public int ShopId { get; set; }

        public Shop Shop { get; set; } = default!;

        public DeleteModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        public IActionResult OnGet()
        {
            Shop = _ShopService.GetShopById(ShopId);

            if (Shop == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnGetDeleteShop()
        {
            bool result = _ShopService.Delete(ShopId);

            if (result)
            {
                return RedirectToPage("/Shops/List");
            }
            else
            {
                return BadRequest(); 
            }
        }
    }
```




## Forsiden

Tilføj dette links til `list.cshtml` så du kan udføre handlingerne på siden:
```html
    <table class="table">
        <tbody>
            @foreach (var Shop in Model.Shops)
            {
                <tr>
                    <td>@Shop.Name</td>
                    <td>@Shop.Location</td>
                    <td>@Shop.Type.Name</td>
                    <td>
                            <a asp-page="/Shops/Detail" asp-route-ShopId="@Shop.ShopId">
                                <i class="fas fa-info-circle"></i>
                            </a>
                    </td>
                    <!--👇 Ny kode 👇-->
                    <td>
                            <a asp-page="/Shops/Delete" asp-route-ShopId="@Shop.ShopId">
                                <i class="fas fa-trash"></i>
                            </a>
                    </td>
                    <td>
                            <a asp-page="/Shops/Edit" asp-route-ShopId="@Shop.ShopId">
                                <i class="fas fa-edit"></i>
                            </a>
                    </td>
                    <!--☝️ Ny kode ☝️-->
                </tr>
            }
        </tbody>
    </table>
```

## I `_Layout.cshtml`

Skift `<header>...<header/>` ud med denne:

```html
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" style="font-family:'Bauhaus 93'; font-size:30px !important" asp-area="" asp-page="/Index">ShopIndex</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-page="/Shops/List">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="btn btn-outline-success nav-link text-dark" asp-area="" asp-page="/Shops/Create"><i class="fa fa-plus"></i> New</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
```




