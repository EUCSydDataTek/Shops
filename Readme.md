# 5. PATCH

I dette eksempel skal vi gøre så vi kan redigere vores shops ved hjælp af `PATCH` metoden.

## Nuget Pakker
For at kunne bruge patch skal vi bruge `Newtonsoft.json` i stedet for `System.text.json`
```pwsh
Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson
```

> ℹ️ Grunden er fordi den normale ikke indeholder de ting man skal bruge for at bruge `PATCH`.
> Vi kommer til det senere i kapitlet hvordan man bruger det.

## I program.cs

Der tilføjes nu en extension metode som skifter `System.text.json` ud med `Newtonsoft.json`.

```c#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddNewtonsoftJson(); // 👈 Tilføj

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IShopService, ShopService>();
```

> ℹ️ Du bruger nu `Newtonsoft.json` i dit projekt i stedet for `System.text.json`. 
> Du vil ikke mærke nogle merkante ændringer efter skiftet da det er det samme bare med flere funktioner.

## Implementering af `PATCH`

Vi kan nu begynde at implementere patch metoden.

I `Newtonsoft.json` har vi et objekt der hedder `JsonPatchDocument` som er en kontext til det patch document vi vil køre på et specifikt objekt.

```C#
    [HttpPatch]
    public IActionResult EditPartially(int shopId,JsonPatchDocument<ShopModel> patchDocument)
    {
        Shop? shop = _shopService.GetShopById(shopId);

        if (shop == null)
        {
            return NotFound();
        }

        ShopModel model = shop.MapToModel();

        try
        {

            patchDocument.ApplyTo(model);

            shop.Location = model.Location;
            shop.Name = model.Name;
            shop.ShopTypeId = model.ShopTypeId;

            _shopService.Update(shop);

        }
        catch (Exception e)
        {
            _Logger.LogError(e.Message);
            return UnprocessableEntity(e.Message);
        }

        return CreatedAtAction("GetShop", new { shopId = model.ShopId },model);
    }
```

## Test

Test med at tilføje dette til .http filen:

```.http
PATCH {{WebApi_HostAddress}}/api/shop?shopId=3
Content-Type: application/json-patch+json

[
    {
        "op": "copy",
        "from": "/Name",
        "path": "/Location",
    },
    {
        "op": "replace",
        "path": "/ShopTypeId",
        "value": 1
    },
    {
        "op": "replace",
        "path": "/Name",
        "value": "Steak n more"
    }
]
```

## Extra

### Debugger til patch documenter
```C#
    private void LogPatchDoc<T>(JsonPatchDocument<T> document) where T : class
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("\n----JsonPatch DEBUG----");

        foreach (var op in document.Operations) {
            sb.Append($"\n-- \n Operation: {op.op}");
            sb.Append($"\n Path: {op.path}");

            switch (op.OperationType)
            {
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Add:
                    sb.Append($"\n Value: {op.value}");
                    break;
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Remove:
                    break;
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Replace:
                    sb.Append($"\n Value: {op.value}");
                    break;
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Move:
                    sb.Append($"\n From: {op.from}");
                    break;
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Copy:
                    sb.Append($"\n From: {op.from}");
                    break;
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Test:
                    sb.Append($"\n Value: {op.value}");
                    break;
                case Microsoft.AspNetCore.JsonPatch.Operations.OperationType.Invalid:
                    break;
            }

            _Logger.LogInformation( sb.ToString() );

        }
    }
```
