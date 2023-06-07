namespace ServiceLayer;
using DataLayer.Entities;

public interface IShopService
{
    public IQueryable<Shop> GetShops();
    public IQueryable<Shop> GetShopsByName(string? name = null);

}
