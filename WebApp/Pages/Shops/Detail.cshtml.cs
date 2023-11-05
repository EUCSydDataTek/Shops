using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class DetailModel : PageModel
    {
        public Shop Shop { get; set; }

        private readonly IShopService _ShopService = default!;

        private readonly IDistributedCache _cache = default!;

        public DetailModel(IShopService shopService,IDistributedCache cache)
        {
            _cache = cache;
            _ShopService = shopService;
        }

        public IActionResult OnGet(int shopId)
        {
            Shop = _ShopService.GetShopById(shopId);

            _cache.SetString("PageLastVisit",shopId.ToString());

            return Page();
        }
    }
}
