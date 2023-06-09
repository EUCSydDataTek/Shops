using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class DetailModel : PageModel
    {
        public Shop Shop { get; set; }

        private readonly IShopService _ShopService = default!;

        public DetailModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        public IActionResult OnGet(int shopId)
        {
            Shop = _ShopService.GetShopById(shopId);
    
            if (Shop == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}
