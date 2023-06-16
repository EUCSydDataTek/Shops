namespace ServiceLayer;
using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

public class ShopService : IShopService
{
    private readonly AppDbContext _AppDbContext = default!;

    public ShopService(AppDbContext appDbContext)
    {
        appDbContext.Database.EnsureCreated();
        _AppDbContext = appDbContext;
    }

    public IQueryable<Shop> GetShops() 
    {
        return _AppDbContext.Shops.Include(s => s.Type).AsNoTracking();
    }

    public IQueryable<Shop> GetShopsByName(string? name = null) 
    {
        return _AppDbContext.Shops
                                .Include(s => s.Type)
                                .Where(s => string.IsNullOrEmpty(name) || s.Name.StartsWith(name))
                                .AsNoTracking();
    }

    public Shop? GetShopById(int ShopId)
    {
        return _AppDbContext.Shops
                            .Include(s => s.Type)
                            .Where(s => s.ShopId == ShopId)
                            .FirstOrDefault();
    }

    public ShopViewModel GetShopsByName(string searchTerm, int currentPage, int pageSize)
    {
        ShopViewModel ShopModel = new();
        var query = _AppDbContext.Shops.Include(s => s.Type).AsNoTracking();
        query = searchTerm != null ? query.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower())).OrderBy(r => r.Name) : query;

        ShopModel.TotalCount = query.Count();

        ShopModel.Shops = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        return ShopModel;
    }

    public Shop Update(Shop updatedShop)
    {
        _AppDbContext.Shops.Update(updatedShop);
            
        _AppDbContext.SaveChanges();

        return updatedShop;
    }  

    public Shop Add(Shop newShop)
    {
        _AppDbContext.Shops.Add(newShop);

        _AppDbContext.SaveChanges();

        return newShop;
    }

    public bool Delete(int ShopId)
    {
        var shop = _AppDbContext.Shops.Where(s => s.ShopId == ShopId).FirstOrDefault();

        if (shop == null)
        {
            return false;
        }

        _AppDbContext.Shops.Remove(shop);

        try
        {
            _AppDbContext.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public IQueryable<ShopType> GetShopTypes()
    {
        return _AppDbContext.ShopTypes.AsNoTracking();
    }
    
    public int GetShopCount()
    {
        return _AppDbContext.Shops.Count();
    }
}
