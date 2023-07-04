using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ServiceLayer;
using WebApi.Mappers;
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

        [HttpGet(Name = "GetShop")]
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

        [HttpPost]
        [Route("create")]
        public IActionResult Create(ShopCreateModel model)
        {
            var NewShop = model.MapToShop();

            try
            {
                _shopService.Add(NewShop);
                return CreatedAtAction("GetShop", new { shopId = NewShop.ShopId }, NewShop.MapToModel());
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult Edit(ShopEditModel model) 
        {
            var shop = model.MaptoShop();

            try
            {
                _shopService.Update(shop);
                return CreatedAtAction("GetShop", new { shopId = shop.ShopId }, shop.MapToModel());
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }       
        }

    }
}


