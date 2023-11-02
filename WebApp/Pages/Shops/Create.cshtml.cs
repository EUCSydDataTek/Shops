using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

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
        public AddShopModel Shop { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; } = default!;

        public void OnGet()
        {
            Types = _ShopService.GetShopTypes()
                    .Select(t => new SelectListItem(t.Name, t.ShopTypeId.ToString()))
                    .ToList();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var newShop = _ShopService.Add(new Shop()
                {
                    Name = Shop.Name,
                    Location = Shop.Location,
                    ShopTypeId = Shop.ShopTypeId,
                });

                return RedirectToPage("/Shops/Detail", new { ShopId = newShop.ShopId });
            }
            else
            {
                return Page();
            }
        }

        public class AddShopModel 
        {

            [Required]
            [StringLength(100)]
            public string Name { get; set; } = string.Empty;

            [Required]
            public int ShopTypeId { get; set; }

            [Required]
            [StringLength(50)]
            public string Location { get; set; } = string.Empty;

        }
    }
}
