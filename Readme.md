# 6. DELETE

I denne demo udvider vi vores `ShopController` med en delete funktion så vi kan fjerne shops fra listen.

## I ShopController

Tilføj dette kode til controlleren:

```C#
    [HttpDelete]
    [Route("remove")]
    public IActionResult Remove(int Shopid)
    {
        var shop = _shopService.GetShopById(Shopid);

        if (shop == null)
            return NotFound();

        try
        {
            bool result = _shopService.Delete(Shopid);

            if (result)
            {
                return NoContent(); // Success
            }
            else
            {
                return UnprocessableEntity(); // Fejl under delete
            }
        }
        catch (Exception e)
        {
            _Logger.LogError(e.Message);
            return UnprocessableEntity(e.Message); // Anden fejl under delete
        }
    }
```

> ℹ️  Når du laver en delete og requesten er fuldført med succes skal man altid returnere 204 (no content) 
> fordi vi har fjernet objektet og så har vi ikke noget at returnere.

## Test

Tilføj dette til Http filen:

```.http
DELETE {{WebApi_HostAddress}}/api/shop/remove?Shopid=2
```