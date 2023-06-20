# 8. Sessions

I denne demo skifter vi cookies ud med en session variabel.

## Webapp

### `Program.cs`
Indsæt disse ting i middleware og services

#### Services
```C#
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

#### Middleware
```C#
app.UseSession();
```

## `Detail.cshtml.cs`
Udskift cookie funktionerne med sessions:
```C#
HttpContext.Session.SetInt32("PageLastVisit", shopId);
```

## `List.cshtml.cs`
```C#
int? shopId = HttpContext.Session.GetInt32("PageLastVisit");

if (shopId != null)
{
    ViewData["PageVisited"] = shopId;
}
```