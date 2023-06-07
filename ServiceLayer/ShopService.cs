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


}
