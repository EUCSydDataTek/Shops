namespace ServiceLayer;
using DataLayer.Entities;

public interface IShopService
{
    public IQueryable<Shop> GetShops();
}
