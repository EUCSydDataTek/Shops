using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ServiceLayer;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public IActionResult GetShop(int shopId)
        {

            Shop? shop = _shopService.GetShopById(shopId);

            if (shop != null)
            {
                var model = new ShopModel() {
                    ShopId = shopId,
                    Name = shop.Name,
                    ShopType = shop.Type.Name,
                    Location = shop.Location,
                    ShopTypeId = shop.ShopTypeId
                };

                return Ok(model);
            } 
            else
            {
                return NotFound();
            }
        }
    }
}


