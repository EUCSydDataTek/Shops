using System.ComponentModel.DataAnnotations;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public IEnumerable<Shop> Shops { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
  
        private readonly IShopService _ShopService = default!;
    
        public ListModel(IShopService ShopService)
        {
            _ShopService = ShopService;
        }
    
        public void OnGet()
        {
            ShopViewModel ShopModel = _ShopService.GetShopsByName(SearchTerm, CurrentPage, PageSize);
            Shops = ShopModel.Shops;
            Count = ShopModel.TotalCount;

            int? shopId = HttpContext.Session.GetInt32("PageLastVisit");

            if (shopId != null)
            {
                ViewData["PageVisited"] = shopId;
            }
        }
    }
}
