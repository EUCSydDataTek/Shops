
# 5.PartialView

I dette eksempel laver vi et partial view til listen med Shops.

## WebApp

lav et razor view med navnet `_ListItemPartial` i mappen `shops`

<br>

`_ListItemPartial.cshtml`

```html
@model DataLayer.Entities.Shop

<tr>
    <td>@Model.Name</td>
    <td>@Model.Location</td>
    <td>@Model.Type.Name</td>
    <td>
        <a asp-page="/Shops/Detail" asp-route-ShopId="@Model.ShopId">
            <i class="fas fa-info-circle"></i>
        </a>
    </td>
    <td>
        <a asp-page="/Shops/Delete" asp-route-ShopId="@Model.ShopId">
            <i class="fas fa-trash"></i>
        </a>
    </td>
    <td>
        <a asp-page="/Shops/Edit" asp-route-ShopId="@Model.ShopId">
            <i class="fas fa-edit"></i>
        </a>
    </td>
</tr>
```

<br>

i `List.cshtml` Fjern derefter koden inden i foreach løkken med det partial vi har lavet.

<br>

`List.cshtml`
```html
<table class="table">
    <tbody>
        @foreach (var Shop in Model.Shops)
        {
            <partial name="_ListItemPartial" model="Shop" /> <!-- 👈 Partial view reference --> 
        }
    </tbody>
</table>
```

> 📘 Du kan også referere med razor metoder
> ```C#
>   @Html.RenderPartial("_ListItemPartial",Shop); // Normal
>   @Html.RenderPartialAsync("_ListItemPartial",Shop); // Async
> ```

