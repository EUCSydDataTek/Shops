# 8. Sessions

I denne demo skifter session variabel ud med In memory caching.

## Webapp

### `Program.cs`
Der behøves ikke at blive oprettet nogle services da servicen for caching er per default indlæst.

## `Detail.cshtml.cs`

Der Med dependecy injection injectes `IMemoryCache` 

```C#
    private readonly IMemoryCache _cache = default!;

    public DetailModel(IShopService shopService,IMemoryCache cache)
    {
        _cache = cache;
        _ShopService = shopService;
    }
```

Udskift session med memoryCache:
```C#
    var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));

    _cache.Set<int>("PageLastVisit",shopId,options);
```

## `List.cshtml.cs`
```C#
int? shopId = HttpContext.Session.GetInt32("PageLastVisit");

if (shopId != null)
{
    ViewData["PageVisited"] = shopId;
}
```

> Det er ikke den bedste metode at bruge caching på da alle kan se hvad den sidste har kigget på!