using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace WebApp.ViewComponents
{
    public class ShopCountViewComponent : ViewComponent
    {
        private readonly IShopService _shopService;

        public ShopCountViewComponent(IShopService shopService)
        {
            _shopService = shopService;
        }

        public IViewComponentResult Invoke()
        {
            int count = _shopService.GetShopCount();
            return View(count);
        }
    }
}
