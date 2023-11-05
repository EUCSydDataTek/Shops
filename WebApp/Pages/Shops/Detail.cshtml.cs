using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class DetailModel : PageModel
    {
        public Shop Shop { get; set; }

        private readonly IShopService _ShopService = default!;

        private readonly IMemoryCache _cache = default!;

        public DetailModel(IShopService shopService,IMemoryCache cache)
        {
            _cache = cache;
            _ShopService = shopService;
        }

        public IActionResult OnGet(int shopId)
        {
            Shop = _ShopService.GetShopById(shopId);

            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set<int>("PageLastVisit",shopId,options);

            return Page();
        }
    }
}
