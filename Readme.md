
# 6.Viewcomponent

I dette eksempel laver vi et view component som viser hvor mange shops der er på listen i Footeren på siden.

![ShopCount](/Images/ShopCount.png)

## ServiceLayer
Lav en metoden `GetShopCount()` som viser hvor mange Shops der er.

`ShopService.cs`
```C#
    public int GetShopCount()
    {
        return _AppDbContext.Shops.Count();
    }
```
> ⚠️ Husk at opdatere interfacet.
## WebApp

I roden af webapp projektet laves en mappe med navn `ViewComponents` inden i den laver du en klasse med navn `ShopCountViewComponent`

<br>

`ShopCountComponent.cs`
```C#
    public class ShopCountViewComponent : ViewComponent
    {
        private readonly IShopService _shopService;

        public ShopCountViewComponent(IShopService shopService)
        {
            _shopService = shopService;
        }

        public IViewComponentResult Invoke()
        {
            int count = _shopService.GetShopCount();
            return View(count);
        }
    }
```

Der laves nu en mappe i `Shared` med navn `Components` og der oprettes en mappe med navnet `ShopCount`. _altså klassen uden `ViewComponent` til sidst_.

Derefter oprettes Razor View med navnet `Default`

>_Eksempel på mappestruktur_
>- Pages
>    - Shared
>        - ShopCount
>            - Default.cshtml

<br>

`Default.cshtml`
```html
    @model int

    <div id="ShopCount">
        <h4>There is @Model Shops</h4>
    </div>
```

### I `_layout.cshtml`

Counteren tilføjes nu til `Footer` af siden:
```html
    <footer class="border-top footer text-muted">
        <div class="container">
            @await Component.InvokeAsync("ShopCount")
        </div>
    </footer>
```



