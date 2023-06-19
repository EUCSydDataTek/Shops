# 7. Cookies

I dette eksempel laver vi en mekanisme som gemmer den sidste du har været inde i details på og makerer den med blå ved hjælp af den bliver gemt i en cookie.

> ### 🍪 Cookie regler
> I europa er der regler om der skal være en promt når bliver gemt. det gælder kun tredjeparts cookies men ikke dem der bliver kaldt funktionelle Cookies da de bliver brugt af siden og er vigtige for siden kan fungere.

<br>

## WebApp

### Detail page

I detail sidens page behind indættes en noget kode der sætter en cookie med navn `PageLastVisit` til det id som du har besøgt sidst.

```C#
    Response.Cookies.Append("PageLastVisit", shopId.ToString());
```

### List Page

I page behind indsæt denne property:
```C#
public int PageVisited { get; set; } = 0;
```

insæt denne kode i `OnGet()`:
```C#
string? VisitorString = string.Empty;
Request.Cookies.TryGetValue("", out VisitorString);

if (VisitorString != null)
{
    x Visited = Convert.ToInt32(VisitorString);
}
```

## _ListItemPartial

før `<tr>` noget der higliter den på siden og ret den første `<tr>`

```html
@model DataLayer.Entities.Shop

@{
    string TrClass = "";

    int? ShopId = Convert.ToInt32(ViewData["PageVisited"]);

    if(ShopId != null)
    {
        if (ShopId == Model.ShopId)
        {
            TrClass = "bg-info";
        }
    }
}

<tr class="@TrClass" >
    ...
```