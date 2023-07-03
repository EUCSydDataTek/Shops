# 2b. Advanced GET

I denne demo er projektet udvidet til at du nu kan lave fristekst søgning samt udføre mere komplekse søgninger.

> ℹ️ Vi bruger stadig de søgefunktioner der er lavet i servicelayer da det bliver mere simpelt og så er der en rød tråd.

# Opret en mapper

For at gøre koden  mere læselig og undgå kode der går igen skal der laves en Mapper der mapper fra Shop til ShopModel.

Der oprettes en mappe der hedder `Mappers` hvor der oprettes en klasse med navn `ShopMapper`

`ShopMapper`:
```C#
 public static class ShopMapper
 {

     public static IEnumerable<ShopModel> MapToModel(this IEnumerable<Shop> shopModels)
     {
         return shopModels.Select(s => new ShopModel()
         {
             Name = s.Name,
             Location = s.Location,
             ShopId = s.ShopId,
             ShopType = s.Type.Name,
             ShopTypeId = s.ShopTypeId
         }).ToList();
     }

     public static ShopModel MapToModel(this Shop shop)
     {
         return new ShopModel()
         {
             Name = shop.Name,
             Location = shop.Location,
             ShopId = shop.ShopId,
             ShopType = shop.Type.Name,
             ShopTypeId = shop.ShopTypeId
         };
     }

 }
```

> ℹ️ Vi kommer til konvertering den anden vej senere.


# Opret en SearchModel

I `Models` opret en klasse kaldet `SearchQueryModel`

`SearchQueryModel`:
```C#
public class SearchQueryModel
{
    public string query { get; set; } = string.Empty;

    public int page { get; set; }

    public int pageSize { get; set; }
}
```

## ShopsController

Opdater ShopsControllerens `GetShops()` metode:
```C#
[HttpGet]
[HttpHead]
public List<ShopModel> GetShops([FromQuery] SearchQueryModel searchQuery)
{
    var model = _shopService.GetShopsByName(searchQuery.query,searchQuery.page,searchQuery.pageSize);

    // Metadata i headeren
    Response.Headers.Add("Page", searchQuery.page.ToString());
    Response.Headers.Add("PageSize", searchQuery.pageSize.ToString());
    Response.Headers.Add("TotalCount", model.TotalCount.ToString());

    return model.Shops.MapToModel().ToList();
}
```
> ℹ️ `HttpHead` gør at du kan requeste med en `HEAD` i stedet for en get.
> - En `HEAD` metode kører koden men returnerer kun Headeren på svaret.
> - God til hvis du kun vil tjække at der er kommet mere data eller et objekt er blevet ændret.

# Test
Test med følgende queries:

- `/api/shops?query=pow`
- `/api/shops?page=1&pagesize=1`
- `/api/shops?query=kage&page=1&pagesize=1`
