namespace ServiceLayer;

using DataLayer.Entities;
public class ShopViewModel
{
    public List<Shop> Shops { get; set; } = default!;
    public int TotalCount { get; set; }
}
