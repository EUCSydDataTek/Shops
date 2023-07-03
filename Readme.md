# 2. GET

I dette eksempel skal vi hente data fra Apien.

kaldet `GET` som er en kommando der signallerer Apien at du vil hente data.

Efter eksmplet er der oprettet kan man hente:
- En Shop med ShopId som parameter.
- En Liste af alle Shops samt ordering og paging.

> ℹ️ Vi bruger servicerne fra Websiden så vi ikke behøver at oprette alt fra bunden.

# Oprettelse af Controllers

### Der oprettes en mappe der hedder `Models` og inde i mappen oprettes en klasse der hedder `ShopModel`

`ShopModel`:
```C#
public class ShopModel
{
    public int ShopId { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string ShopType { get; set; }

    public int ShopTypeId { get; set; }
}
```

### WeatherforcastController fjernes og der oprettes følgende controllere:
- `ShopController`
- `ShopsController`

> ℹ️ Grunden til at shops har 2 Contollers er fordi at der skal laves en kontroller per substantiv af data som er:
> - Ental = henter,tilføjer,redigerer eller fjerner et objekt.
> - flertal = Liste af flere objekter.

Følgende indhold tilføjes til controllerne:

`ShopController`:
```C#
[Route("api/[controller]")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpGet]
    public IActionResult GetShop(int shopId)
    {

        Shop? shop = _shopService.GetShopById(shopId);

        if (shop != null)
        {
            var model = new ShopModel() {
                ShopId = shopId,
                Name = shop.Name,
                ShopType = shop.Type.Name,
                Location = shop.Location,
                ShopTypeId = shop.ShopTypeId
            };

            return Ok(model);
        } 
        else
        {
            return NotFound();
        }
    }
}
```

`ShopsController`:
```C#
[Route("api/[controller]")]
[ApiController]
public class ShopsController : ControllerBase
{

    private readonly IShopService _shopService;

    public ShopsController(IShopService shopService)
    {
        _shopService = shopService;
    }

    public List<ShopModel> GetShops()
    {
        return _shopService.GetShops().Select(s => new ShopModel()
        {
            Name = s.Name,
            Location = s.Location,
            ShopId = s.ShopId,
            ShopType = s.Type.Name,
            ShopTypeId = s.ShopTypeId
        }).ToList();
    }

}
```

# Test Apien

Start Apien og test mmed føgende adresser:
- `/api/shop?ShopId=5`
- `/api/shops`