using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace MyApp.Namespace
{
    public class ListModel : PageModel
    {
        public IEnumerable<Shop> Shops { get; set; }

        [BindProperty(SupportsGet = true)] // 👈 Ny
        public string SearchTerm { get; set; } // 👈 Ny
  
        private readonly IShopService _ShopService = default!;
    
        public ListModel(IShopService ShopService)
        {
            _ShopService = ShopService;
        }
    
    
        public void OnGet()
        {
            Shops = _ShopService.GetShopsByName(SearchTerm).ToList();
        }
    }
}
