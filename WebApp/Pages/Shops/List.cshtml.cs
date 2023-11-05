using System.ComponentModel.DataAnnotations;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class ListModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageVisited { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int Count { get; set; }

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public enum PageSizeEnum
        {
            [Display(Name = "2")]
            _2 = 2,
            [Display(Name = "4")]
            _4 = 4,
            [Display(Name = "10")]
            _10 = 10,
        }

        public IEnumerable<Shop> Shops { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = default!;
  
        private readonly IShopService _ShopService = default!;
    
        private readonly IDistributedCache _cache = default!;

        public ListModel(IShopService shopService,IDistributedCache cache)
        {
            _cache = cache;
            _ShopService = shopService;
        }
    
        public void OnGet()
        {
            ShopViewModel ShopModel = _ShopService.GetShopsByName(SearchTerm, CurrentPage, PageSize);
            Shops = ShopModel.Shops;
            Count = ShopModel.TotalCount;

            int shopId = Convert.ToInt32(_cache.GetString("PageLastVisit"));
            ViewData["PageVisited"] = shopId;
            
        }
    }
}
