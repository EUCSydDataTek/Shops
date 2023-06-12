using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;

namespace WebApp.Pages.Shops
{
    public class EditModel : PageModel
    {
        private readonly IShopService _ShopService;

        public int ShopId { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; } = default!;

        [BindProperty]
        public Shop Shop { get; set; } = default!;

        public EditModel(IShopService shopService)
        {
            _ShopService = shopService;
        }

        public IActionResult OnGet()
        {
            Shop = _ShopService.GetShopById(ShopId);

            if(Shop == null)
            {
                return RedirectToPage("./NotFound");
            }

            Types = _ShopService.GetShopTypes()
                                .Select(t => new SelectListItem(t.Name,t.ShopTypeId.ToString()))
                                .ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                _ShopService.Update(Shop);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Page();
        }
    }
}
