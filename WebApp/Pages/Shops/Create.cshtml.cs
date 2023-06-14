using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class CreateModel : PageModel
    {
        private readonly IShopService _ShopService;

        public CreateModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        [BindProperty]
        public Shop Shop { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; } = default!;

        public void OnGet()
        {
            Types = _ShopService.GetShopTypes()
                    .Select(t => new SelectListItem(t.Name, t.ShopTypeId.ToString()))
                    .ToList();
        }

        public IActionResult OnPost()
        {
            var newShop = _ShopService.Add(Shop);

            return RedirectToPage("/Shops/Detail", new { ShopId = Shop.ShopId });
        }
    }
}
