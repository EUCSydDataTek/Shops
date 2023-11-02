using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

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

            byte[] ImageData = default!;

            if (Shop?.Image != null)
            {

                using (MemoryStream stream = new MemoryStream())
                {
                    
                    string Fullpath = Path.Combine("wwwroot","Images", Shop.Image.FileName);

                    Shop.Image.CopyTo(stream);

                    if (stream.Length <= 4000000)
                    {
                        ImageData = stream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(Shop.Image), "Filen skal være max. 4Mb.");
                    }
                    
                }
            }

            if (ModelState.IsValid)
            {
                

                var newShop = _ShopService.Add(new Shop()
                {
                    Name = Shop.Name,
                    Location = Shop.Location,
                    ShopTypeId = Shop.ShopTypeId,
                    ImageMimeType = Shop.Image.ContentType,
                    ImageData = ImageData
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

            [Required(ErrorMessage = "* Name skal udfyldes.")]
            [StringLength(100)]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "* ShopType skal udfyldes.")]
            public int ShopTypeId { get; set; }

            [Required(ErrorMessage = "* Location skal udfyldes.")]
            [MaxLength(50,ErrorMessage = "Location skal maximum være 50 karakterer lang")]
            public string Location { get; set; } = string.Empty;

            public IFormFile Image { get; set; } = default!;

        }
    }
}
