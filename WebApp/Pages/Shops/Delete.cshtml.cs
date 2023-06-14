using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class DeleteModel : PageModel
    {
        private readonly IShopService _ShopService;

        [BindProperty(SupportsGet = true)]
        public int ShopId { get; set; }

        public Shop Shop { get; set; } = default!;

        public DeleteModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        public IActionResult OnGet()
        {
            Shop = _ShopService.GetShopById(ShopId);

            if (Shop == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnGetDeleteShop()
        {
            bool result = _ShopService.Delete(ShopId);

            if (result)
            {
                return RedirectToPage("/Shops/List");
            }
            else
            {
                return BadRequest(); 
            }
        }
    }
}
