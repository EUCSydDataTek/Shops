# 5. PUT

I dette eksempel skal vi gøre så vi kan redigere vores shops ved hjælp af `PUT` metoden.

## Oprettelse af model

Der oprettes en model der hedder `ShopEditModel` som bruges som en form for editform

`ShopEditModel`:

```C#
public class ShopEditModel
{
    public int ShopId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int ShopTypeId { get; set; }

    public string Location { get; set; } = string.Empty;
}
```

## Mapping

`ShopMapper` udvides så man kan konvertere en `ShopEditModel` til et `Shop` objekt.

`ShopMapper`:
```C#
public static Shop MaptoShop(this ShopEditModel model)
{
    return new Shop()
    {
        Name = model.Name,
        Location = model.Location,
        ShopTypeId = model.ShopTypeId,
        ShopId = model.ShopId,
    };
}
```

## Controller

Udvid `Shopcontroller` med en ny metode:
```C#
[HttpPut]
[Route("edit")]
public IActionResult Edit(ShopEditModel model) 
{
    var shop = model.MaptoShop();

    try
    {
        _shopService.Update(shop);
        return CreatedAtAction("EditShop", new { shopId = shop.ShopId }, shop.MapToModel());
    }
    catch (Exception e)
    {
        return UnprocessableEntity(e.Message);
    }       
}
```

## Test

Tilføj til `.http` filen:
```http
PUT {{WebApi_HostAddress}}/api/shop/edit
Content-Type: application/json

{
  "ShopId":1,
  "Name":"Poer",
  "ShopTypeId":3,
  "Location":"Nowhere"
}

###
```