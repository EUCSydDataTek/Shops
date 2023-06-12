namespace ServiceLayer;
using DataLayer.Entities;

public interface IShopService
{
    public IQueryable<Shop> GetShops();
    public IQueryable<Shop> GetShopsByName(string? name = null);
    public Shop? GetShopById(int ShopId);
    public ShopViewModel GetShopsByName(string searchTerm, int currentPage, int pageSize);
    public Shop Update(Shop updatedShop);
    public Shop Add(Shop newShop);
    public IQueryable<ShopType> GetShopTypes();
}
