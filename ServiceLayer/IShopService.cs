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
    public bool Delete(int ShopId);
    public IQueryable<ShopType> GetShopTypes();
    public int GetShopCount();
}
