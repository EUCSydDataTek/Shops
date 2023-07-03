using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {

        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public List<ShopModel> GetShops()
        {
            return _shopService.GetShops().Select(s => new ShopModel()
            {
                Name = s.Name,
                Location = s.Location,
                ShopId = s.ShopId,
                ShopType = s.Type.Name,
                ShopTypeId = s.ShopTypeId
            }).ToList();
        }

    }
}
