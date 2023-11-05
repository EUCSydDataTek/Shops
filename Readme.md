# 8. Sessions

I denne demo skifter session variabel ud med In memory caching.

## Webapp

### `Program.cs`
Tilføj distributed memorycache service
```C#
builder.Services.AddDistributedMemoryCache();
```

## `Detail.cshtml.cs`

Der Med dependecy injection injectes `IMemoryCache` 

```C#
    private readonly IDistributedCache _cache = default!;

    public DetailModel(IShopService shopService,IDistributedCache cache)
    {
        _cache = cache;
        _ShopService = shopService;
    }
```

Udskift session med memoryCache:
```C#
    _cache.SetString("PageLastVisit",shopId.ToString());
```
> Da det er en distributed cache vil den kun tage imod `string` eller `byte[]`

## `List.cshtml.cs`
```C#
    int shopId = Convert.ToInt32(_cache.GetString("PageLastVisit"));
    ViewData["PageVisited"] = shopId;
```

> Denne metode kan modre end memory cache men den er mere robust og kan udvides med en cache server (Næste branch)