# 3. POST

I dette eksempel skal vi implementere så man kan oprette en shop.

## Oprettelse af model

I mappen `Models` oprettes en klasse med navn `ShopCreateModel`

`ShopCreateModel`:
```C#
public class ShopCreateModel
{
    public string Name { get; set; }

    public string Location { get; set; }

    public int ShopTypeId { get; set; }
}
```

> ℹ️ Vi bruger en model fordi vi ikke vil have at brugeren kan redigere i `ShopId` eller andet som ikke skal udføres under en oprettelse.

## ShopMapper

I `ShopMapper` klassen tilføjes en ny metode til at konvertere til et `Shop` objekt .

`ShopMapper`:
```C#
public static Shop MapToShop(this ShopCreateModel model)
{
    return new Shop()
    {
        Name = model.Name,
        Location = model.Location,
        ShopTypeId = model.ShopTypeId,
    };
}
```

> ℹ️ Man kunne ligså godt have brugt AutoMapper her.
> Men fordi projektet er i denne størrelse kan vi godt gøre det manuelt.

## Oprettelse af POST metode i Controller

I `ShopController` rettes `HttpGet` på `GetShop` så den ser sådan ud:
```C#
[HttpGet(Name = "GetShop")] 
public IActionResult GetShop(int shopId)
{
    ...
}
```

> ℹ️ Navnet vi giver i name er en form for pointer til denne metode så vi både kan bruge den til redirects eller til brug i en `CreatedAtAction` metode.

<br>

`ShopController`:
```C#
[HttpPost]
[Route("create")]
public IActionResult Create(ShopCreateModel model)
{
    var NewShop = model.MapToShop();

    try
    {
        _shopService.Add(NewShop);
        return CreatedAtAction("GetShop",new { shopId = NewShop.ShopId },NewShop.MapToModel());
    }
    catch (Exception e)
    {
        return UnprocessableEntity(e.Message);
    }
}
```

> ℹ️ `[Route]` bruges til at sige at vi gerne vil have at den har sin egen route.
> i dette tilfælde er det `/api/shop/create`